import axios from "axios";
import { baseUrl } from "@/config/constants";
import { CREATE_MATERIAL_FAILURE, CREATE_MATERIAL_REQUEST, CREATE_MATERIAL_SUCCESS, DELETE_MATERIAL_FAILURE, DELETE_MATERIAL_REQUEST, DELETE_MATERIAL_SUCCESS, EXTRACT_VIDEO_ID_BY_URL_FAILURE, EXTRACT_VIDEO_ID_BY_URL_REQUEST, EXTRACT_VIDEO_ID_BY_URL_SUCCESS, GET_ALL_MATERIAL_THEMES_FAILURE, GET_ALL_MATERIAL_THEMES_REQUEST, GET_ALL_MATERIAL_THEMES_SUCCESS, GET_ALL_MATERIAL_TYPES_FAILURE, GET_ALL_MATERIAL_TYPES_REQUEST, GET_ALL_MATERIAL_TYPES_SUCCESS, GET_ALL_MATERIALS_FAILURE, GET_ALL_MATERIALS_REQUEST, GET_ALL_MATERIALS_SUCCESS, UPDATE_MATERIAL_FAILURE, UPDATE_MATERIAL_REQUEST, UPDATE_MATERIAL_SUCCESS } from "./ActionType";

export const getAllMaterials = () => async (dispatch) => {
    dispatch({ type: GET_ALL_MATERIALS_REQUEST });

    try {
        const response = await axios.get(`${baseUrl}/api/Material/get-materials`, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        console.log(response);
        dispatch({ type: GET_ALL_MATERIALS_SUCCESS, payload: response.data, success: response?.data?.success });
    } catch (e) {
        console.log(e);
        dispatch({ type: GET_ALL_MATERIALS_FAILURE, payload: e.response?.data?.validationErrors });
    }
}

export const getAllMaterialTypes = () => async (dispatch) => {
    dispatch({ type: GET_ALL_MATERIAL_TYPES_REQUEST });

    try {
        const response = await axios.get(`${baseUrl}/api/Material/get-materials-types`, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        dispatch({ type: GET_ALL_MATERIAL_TYPES_SUCCESS, payload: response.data?.data, success: response?.data?.success });
    } catch (e) {
        console.log(e);
        dispatch({ type: GET_ALL_MATERIAL_TYPES_FAILURE, payload: e.response?.data?.validationErrors });
    }
}

export const getAllMaterialThemes = () => async (dispatch) => {
    dispatch({ type: GET_ALL_MATERIAL_THEMES_REQUEST });

    try {
        const response = await axios.get(`${baseUrl}/api/Material/get-materials-themes`, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        dispatch({ type: GET_ALL_MATERIAL_THEMES_SUCCESS, payload: response.data?.data, success: response?.data?.success });
    } catch (e) {
        console.log(e);
        dispatch({ type: GET_ALL_MATERIAL_THEMES_FAILURE, payload: e.response?.data?.validationErrors });
    }
}

export const createMaterial = (data) => async (dispatch) => {
    dispatch({ type: CREATE_MATERIAL_REQUEST });

    try {
        const response = await axios.post(`${baseUrl}/api/Material/create-material`, data, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        console.log(response);
        dispatch({ type: CREATE_MATERIAL_SUCCESS, payload: response.data });
        return { success: true };
    } catch (e) {
        console.log(e);
        dispatch({ type: CREATE_MATERIAL_FAILURE, payload: e.response?.data?.validationErrors });
        return { success: false };
    }
}

export const updateMaterial = (id, patches) => async (dispatch) => {
    dispatch({ type: UPDATE_MATERIAL_REQUEST });

    try {
        const response = await axios.patch(`${baseUrl}/api/Material/update-material-by/${id}`, patches, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        console.log(response);
        dispatch({ type: UPDATE_MATERIAL_SUCCESS, payload: response.data });
        return { success: true };
    } catch (e) {
        console.log(e);
        dispatch({ type: UPDATE_MATERIAL_FAILURE, payload: e.response?.data?.validationErrors });
        return { success: false };
    }
}

export const deleteMaterial = (id) => async (dispatch) => {
    dispatch({ type: DELETE_MATERIAL_REQUEST });

    try {
        const response = await axios.delete(`${baseUrl}/api/Material/delete-material-by/${id}`, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        console.log(response);
        dispatch({ type: DELETE_MATERIAL_SUCCESS, payload: id });
    } catch (e) {
        console.log(e);
        dispatch({ type: DELETE_MATERIAL_FAILURE, payload: e.response?.data?.validationErrors });
    }
}

export const extractVideoIdByUrl = (url) => async (dispatch) => {
    dispatch({ type: EXTRACT_VIDEO_ID_BY_URL_REQUEST });

    try {
        const response = await axios.get(`${baseUrl}/api/Material/extract-youtube-id?url=${url}`, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        dispatch({ type: EXTRACT_VIDEO_ID_BY_URL_SUCCESS, payload: response.data.videoId });
    } catch (e) {
        console.log(e);
        dispatch({ type: EXTRACT_VIDEO_ID_BY_URL_FAILURE, payload: e.response?.data?.validationErrors });
    }
}