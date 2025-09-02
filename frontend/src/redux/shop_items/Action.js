import axios from "axios";
import { baseUrl } from "@/config/constants";
import { CREATE_AVATAR_FAILURE, CREATE_AVATAR_REQUEST, CREATE_AVATAR_SUCCESS, DELETE_AVATAR_FAILURE, DELETE_AVATAR_REQUEST, DELETE_AVATAR_SUCCESS, GET_ALL_AVATARS_FAILURE, GET_ALL_AVATARS_REQUEST, GET_ALL_AVATARS_SUCCESS, GET_USER_PURCHASES_FAILURE, GET_USER_PURCHASES_REQUEST, GET_USER_PURCHASES_SUCCESS, UPDATE_AVATAR_FAILURE, UPDATE_AVATAR_REQUEST, UPDATE_AVATAR_SUCCESS } from "./ActionType";
import { toast } from "react-toastify";

export const createAvatar = (data) => async (dispatch) => {
    dispatch({ type: CREATE_AVATAR_REQUEST });

    try {
        const response = await axios.post(`${baseUrl}/api/ShopItems/create-avatar`, data, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        console.log(response);
        dispatch({ type: CREATE_AVATAR_SUCCESS, payload: response.data, success: response?.data?.success });
        return { success: true };
    } catch (e) {
        console.log(e);
        dispatch({ type: CREATE_AVATAR_FAILURE, payload: e.response?.data?.validationErrors });
        return { success: false };
    }
}

export const getAllAvatars = () => async (dispatch) => {
    dispatch({ type: GET_ALL_AVATARS_REQUEST });

    try {
        const response = await axios.get(`${baseUrl}/api/ShopItems/get-all-avatars`, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        console.log(response);
        dispatch({ type: GET_ALL_AVATARS_SUCCESS, payload: response?.data, success: response?.data?.success });
        toast.success(response?.data?.message);
    } catch (e) {
        console.log(e);
        dispatch({ type: GET_ALL_AVATARS_FAILURE, payload: e.response?.data?.validationErrors });
        toast.error(e.response?.data?.message);
    }
}

export const updateAvatar = (id, patches) => async (dispatch) => {
    dispatch({ type: UPDATE_AVATAR_REQUEST });

    try {
        const response = await axios.patch(`${baseUrl}/api/ShopItems/update-avatar/${id}`, patches, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        dispatch({ type: UPDATE_AVATAR_SUCCESS, payload: id });
        toast.success("Тест оновлена успішно!");
        return { success: true };
    } catch (e) {
        console.log(e);
        dispatch({ type: UPDATE_AVATAR_FAILURE });
        toast.error(e?.response?.data?.message || "Невдалося оновити аватар!");
        return { success: false };
    }
}

export const deleteAvatar = (id) => async (dispatch) => {
    dispatch({ type: DELETE_AVATAR_REQUEST });

    try {
        const response = await axios.delete(`${baseUrl}/api/ShopItems/delete-avatar/${id}`, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        dispatch({ type: DELETE_AVATAR_SUCCESS, payload: id });
        toast.success(response?.data?.message || "Аватар видалено успішно!");
    } catch (e) {
        console.log(e);
        dispatch({ type: DELETE_AVATAR_FAILURE });
        toast.error(e?.response?.data?.message || "Невдалося видалити аватар!");
    }
}

export const buyAvatar = (data) => async (dispatch) => {
    dispatch({ type: CREATE_AVATAR_REQUEST });

    try {
        const response = await axios.post(`${baseUrl}/api/ShopItems/buy-item`, data, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        console.log(response);
        dispatch({ type: CREATE_AVATAR_SUCCESS, payload: response.data, success: response?.data?.success });
        return { success: true };
    } catch (e) {
        console.log(e);
        dispatch({ type: CREATE_AVATAR_FAILURE, payload: e.response?.data?.validationErrors });
        return { success: false };
    }
}

export const getUserPurchases = (userId) => async (dispatch) => {
    dispatch({ type: GET_USER_PURCHASES_REQUEST });

    try {
        const response = await axios.get(`${baseUrl}/api/ShopItems/get-user-purchases/${userId}`, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        console.log(response);
        dispatch({ type: GET_USER_PURCHASES_SUCCESS, payload: response?.data, success: response?.data?.success });
    } catch (e) {
        console.log(e);
        dispatch({ type: GET_USER_PURCHASES_FAILURE, payload: e.response?.data?.validationErrors });
    }
}