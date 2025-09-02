import { baseUrl } from "@/config/constants";
import { GET_CURRENT_USER_FAILURE, GET_CURRENT_USER_REQUEST, GET_CURRENT_USER_SUCCESS, LOGIN_FAILURE, LOGIN_REQUEST, LOGIN_SUCCESS, LOGOUT, REGISTER_FAILURE, REGISTER_REQUEST, REGISTER_SUCCESS, RESET_PASSWORD_FAILURE, RESET_PASSWORD_REQUEST, RESET_PASSWORD_SUCCESS, SEND_RESET_LINK_FAILURE, SEND_RESET_LINK_REQUEST, SEND_RESET_LINK_SUCCESS, VERIFY_FAILURE, VERIFY_REQUEST, VERIFY_SUCCESS } from "./ActionType";
import axios from "axios";
import { toast } from "react-toastify";

export const register = (data) => async (dispatch) => {
    dispatch({ type: REGISTER_REQUEST });

    try {
        const response = await axios.post(`${baseUrl}/api/Auth/register`, data, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        console.log(response);
        dispatch({ type: REGISTER_SUCCESS, payload: data, success: response.data.success });
        toast.success(response.data.message);
    } catch (e) {
        console.log(e);
        dispatch({ type: REGISTER_FAILURE, payload: e.response?.data?.validationErrors });
        toast.error(e.response?.data?.message);
    }
};

export const login = (data) => async (dispatch) => {
    dispatch({ type: LOGIN_REQUEST });

    try {
        const response = await axios.post(`${baseUrl}/api/Auth/login`, data, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        dispatch({ type: LOGIN_SUCCESS, payload: data });
        await dispatch(getCurrentUser());
        toast.success(response.data.message);
    } catch (e) {
        dispatch({ type: LOGIN_FAILURE, payload: e.response?.data?.validationErrors });
        toast.error(e.response?.data?.message);
    }
};

export const logout = () => async (dispatch) => {
    dispatch({ type: LOGOUT });

    try {
        const response = await axios.post(`${baseUrl}/api/Auth/logout`, {}, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        dispatch({ type: LOGOUT, payload: response.data });
        toast.success(response.data.message);
    } catch (e) {
        dispatch({ type: LOGOUT, payload: e.response?.data?.validationErrors });
        toast.error(e.response?.data?.message);
    }
}

export const verifyCode = (data) => async (dispatch) => {
    dispatch({ type: VERIFY_REQUEST });
    console.log("Verify Code Action", data);
    try {
        const response = await axios.post(`${baseUrl}/api/Auth/verify`, data, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        dispatch({ type: VERIFY_SUCCESS, payload: response.data });
        dispatch(getCurrentUser());
        toast.success(response.data.message);
    } catch (e) {
        console.log(e);
        dispatch({ type: VERIFY_FAILURE, payload: e.response?.data?.validationErrors });
        toast.error(e.response?.data?.message);
    }
}

export const getCurrentUser = () => async (dispatch) => {
    dispatch({ type: GET_CURRENT_USER_REQUEST });
    console.log("Fetching current user");
    try {
        const response = await axios.get(`${baseUrl}/api/Auth/get-current-user`, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        console.log(response);
        dispatch({ type: GET_CURRENT_USER_SUCCESS, payload: response.data });
    } catch (e) {
        console.log(e);
        dispatch({ type: GET_CURRENT_USER_FAILURE });
    }
}

export const sendResetLink = (data) => async (dispatch) => {
    dispatch({ type: SEND_RESET_LINK_REQUEST });

    try {
        const response = await axios.post(`${baseUrl}/api/Auth/send-reset-link`, data, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        dispatch({ type: SEND_RESET_LINK_SUCCESS, payload: response.data });
        toast.success(response.data?.message || "Посилання на скидання пароля надіслано на вашу електронну пошту!");
    } catch (e) {
        dispatch({ type: SEND_RESET_LINK_FAILURE });
        toast.error(e.response?.data || "Сталася помилка при надсиланні посилання на скидання пароля");
    }
}

export const resetPassword = (data) => async (dispatch) => {
    dispatch({ type: RESET_PASSWORD_REQUEST });

    try {
        const response = await axios.patch(`${baseUrl}/api/Auth/update-password`, data, {
            params: { token: data.token },
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        dispatch({ type: RESET_PASSWORD_SUCCESS, payload: response.data });
        toast.success(response?.data.message || "Пароль успішно оновлений!");
    } catch (e) {
        dispatch({ type: RESET_PASSWORD_FAILURE });
        toast.error(e?.response?.data || "Сталася помилка при оновленні пароля");
    }
}