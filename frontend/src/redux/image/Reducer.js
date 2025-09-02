import {
    GET_IMAGE_BY_ID_FAILURE,
    GET_IMAGE_BY_ID_REQUEST,
    GET_IMAGE_BY_ID_SUCCESS,
    SET_IMAGE_BASE64,
} from "./ActionType";

const initialState = {
    image: null,             // може містити JSON-інфу про зображення
    imageBase64: null,       // окреме поле для Base64
    loading: false,
    success: false,
    errors: [],
};

export const imageReducer = (state = initialState, action) => {
    switch (action.type) {
        case GET_IMAGE_BY_ID_REQUEST:
            return {
                ...state,
                loading: true,
                success: false,
                errors: [],
            };
        case GET_IMAGE_BY_ID_SUCCESS:
            return {
                ...state,
                image: action.payload,
                loading: false,
                success: action.success ?? true,
                errors: [],
            };
        case GET_IMAGE_BY_ID_FAILURE:
            return {
                ...state,
                loading: false,
                success: false,
                errors: action.payload || [],
            };
        case SET_IMAGE_BASE64:
            return {
                ...state,
                imageBase64: action.payload,
            };
        default:
            return state;
    }
};