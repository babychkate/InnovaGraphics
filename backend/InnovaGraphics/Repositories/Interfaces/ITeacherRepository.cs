namespace InnovaGraphics.Repositories.Interfaces
{
    public interface ITeacherRepository
    {
        Task<bool> CheckIfTeacherExistsByEmailAsync(string email);
    }
}
