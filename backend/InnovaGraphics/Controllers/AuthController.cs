using Azure.Core;
using InnovaGraphics.Dtos;
using InnovaGraphics.Interactions;
using InnovaGraphics.Utils.Facade;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InnovaGraphics.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthFacade _authFacade;

        public AuthController(AuthFacade authFacade)
        {
            _authFacade = authFacade;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            if (HttpContext.Session == null || !HttpContext.Session.IsAvailable)
            {
                return StatusCode(500, new Response
                {
                    Success = false,
                    Message = "Сесія користувача недоступна."
                });
            }

            var registrationResult = await _authFacade.RegisterUserAsync(request);
            var result = registrationResult.Result;
            var processId = registrationResult.ProcessId;

            if (result.Success)
            {
                if (!string.IsNullOrEmpty(processId))
                {
                    HttpContext.Session.SetString("RegistrationProcessId", processId);
                }

                return Ok(result);
            }
            else
            {
                return StatusCode(result.StatusCode, result);
            }
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyCodeDto request)
        {
            if (HttpContext.Session == null || !HttpContext.Session.IsAvailable)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    Success = false,
                    Message = "Сеанс користувача недоступний."
                });
            }

            var processId = HttpContext.Session.GetString("RegistrationProcessId");

            if (string.IsNullOrEmpty(processId))
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Response
                {
                    Success = false,
                    Message = "Не знайдено активного запиту на реєстрацію. Спробуйте зареєструватися знову."
                });
            }

            var registrationCompletionResult = await _authFacade.CompleteRegistrationAsync(processId, request.Code);

            var result = registrationCompletionResult.Result;
            var accessToken = registrationCompletionResult.AccessToken;
            var refreshToken = registrationCompletionResult.RefreshToken;
            var createdUser = registrationCompletionResult.User;

            HttpContext.Session.Remove("RegistrationProcessId");

            if (result.Success && !string.IsNullOrEmpty(accessToken) && !string.IsNullOrEmpty(refreshToken))
            {
                var accessTokenCookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddHours(1)
                };
                Response.Cookies.Append("access_token", accessToken, accessTokenCookieOptions);

                var refreshTokenCookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, 
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(7) 
                };
                Response.Cookies.Append("refresh_token", refreshToken, refreshTokenCookieOptions);

                return Ok(new
                {
                    message = result.Message
                });
            }
            else
            {
                return StatusCode(result.StatusCode, result);
            }
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (response, accessToken, refreshToken) = await _authFacade.LoginAsync(model.Email, model.Password);

            if (!response.Success)
                return StatusCode(response.StatusCode, response);

            var accessTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            var refreshTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("access_token", accessToken, accessTokenOptions);
            Response.Cookies.Append("refresh_token", refreshToken, refreshTokenOptions);

            return Ok(new
            {
                Message = response.Message
            });
        }

        [HttpGet("get-current-user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (userEmail == null)
            {
                return Unauthorized(new
                {
                    Message = "Не вдалося ідентифікувати користувача за токеном"
                });
            }

            var userDto = await _authFacade.GetUserDtoByEmailAsync(userEmail);

            if (userDto == null)
            {
                return NotFound(new
                {
                    Message = "Користувача не знайдено"
                });
            }

            return Ok(userDto);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            if (Request.Cookies.ContainsKey("access_token"))
                if (Request.Cookies.ContainsKey("refresh_token"))
                {
                    Response.Cookies.Append("access_token", "", new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTime.UtcNow.AddDays(-1)
                    });
                    var refreshTokenValue = Request.Cookies["refresh_token"];

                    bool revoked = await _authFacade.RevokeRefreshTokenAsync(refreshTokenValue);
                }

            Response.Cookies.Append("access_token", "", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(-1)
            });

            Response.Cookies.Append("refresh_token", "", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(-1)
            });

            return Ok(new { Message = "Ви успішно вийшли з системи." });
        }

        [HttpPost("send-reset-link")]
        public async Task<IActionResult> SendResetLink([FromBody] EmailDto emailDto)
        {
            if (emailDto == null || string.IsNullOrEmpty(emailDto.Email))
            {
                return BadRequest(new Response
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Необхідно ввести адресу електронної пошти"
                });
            }

            var response = await _authFacade.SendPasswordResetLink(emailDto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPatch("update-password")]
        public async Task<IActionResult> UpdatePassword([FromQuery] string token, [FromBody] UpdatePasswordDto dto)
        {
            if (string.IsNullOrEmpty(token) || dto == null || string.IsNullOrEmpty(dto.NewPassword) || string.IsNullOrEmpty(dto.ConfirmNewPassword))
            {
                return BadRequest(new Response
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Необхідно заповнити всі поля."
                });
            }

            var response = await _authFacade.UpdatePassword(token, dto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var response = await _authFacade.RefreshTokenAsync(Request);

            if (!response.Success)
                return StatusCode(response.StatusCode, response);

            if (response is RefreshTokenResponse refreshResponse)
            {
                var accessTokenOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddHours(1)
                };
                Response.Cookies.Append("access_token", refreshResponse.AccessToken, accessTokenOptions);

                var refreshTokenOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = refreshResponse.RefreshTokenExpires
                };
                Response.Cookies.Append("refresh_token", refreshResponse.NewRefreshToken, refreshTokenOptions);

                return Ok(new
                {
                    message = refreshResponse.Message,
                    accessToken = refreshResponse.AccessToken,
                    refreshToken = refreshResponse.NewRefreshToken
                });
            }

            return Ok(response);
        }
    }
}

