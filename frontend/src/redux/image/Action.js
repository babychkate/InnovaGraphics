import { baseUrl } from "@/config/constants";
import { GET_IMAGE_BY_ID_FAILURE, GET_IMAGE_BY_ID_REQUEST, GET_IMAGE_BY_ID_SUCCESS, SET_IMAGE_BASE64 } from "./ActionType"
import axios from "axios";

export const getImageById = (id) => async (dispatch) => {
    dispatch({ type: GET_IMAGE_BY_ID_REQUEST });

    try {
        const response = await axios.get(`${baseUrl}/api/Images/${id}`, {
            headers: {
                'Content-Type': 'application/json'
            },
            responseType: 'blob',
            withCredentials: true,
        });
        console.log(response);
        dispatch({ type: GET_IMAGE_BY_ID_SUCCESS, payload: response });
        dispatch(setImageBase64(response.data));
    } catch (e) {
        console.log(e);
        dispatch({ type: GET_IMAGE_BY_ID_FAILURE });
    }
}

export const setImageBase64 = (blob) => async (dispatch) => {
    try {
        const reader = new FileReader();
        reader.readAsDataURL(blob);
        reader.onloadend = () => {
            const base64data = reader.result;
            dispatch({ type: SET_IMAGE_BASE64, payload: base64data });
        };
    } catch (e) {
        console.error("Помилка при конвертації зображення в Base64", e);
    }
};
