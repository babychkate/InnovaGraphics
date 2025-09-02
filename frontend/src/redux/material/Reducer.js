import { DELETE_MATERIAL_REQUEST, DELETE_MATERIAL_SUCCESS, EXTRACT_VIDEO_ID_BY_URL_FAILURE, EXTRACT_VIDEO_ID_BY_URL_REQUEST, EXTRACT_VIDEO_ID_BY_URL_SUCCESS, GET_ALL_MATERIAL_THEMES_FAILURE, GET_ALL_MATERIAL_THEMES_REQUEST, GET_ALL_MATERIAL_THEMES_SUCCESS, GET_ALL_MATERIAL_TYPES_FAILURE, GET_ALL_MATERIAL_TYPES_REQUEST, GET_ALL_MATERIAL_TYPES_SUCCESS, GET_ALL_MATERIALS_FAILURE, GET_ALL_MATERIALS_REQUEST, GET_ALL_MATERIALS_SUCCESS } from "./ActionType";

const initialState = {
    material: null,
    materials: [],
    videoId: null,
    types: [],
    themes: [],
    loading: false,
    success: false,
    errors: [],
};

export const materialReducer = (state = initialState, action) => {
    switch (action.type) {
        case GET_ALL_MATERIALS_REQUEST:
        case DELETE_MATERIAL_REQUEST:
        case GET_ALL_MATERIAL_TYPES_REQUEST:
        case GET_ALL_MATERIAL_THEMES_REQUEST:
        case EXTRACT_VIDEO_ID_BY_URL_REQUEST:
            return {
                ...state,
                loading: true,
                success: false,
                errors: [],
            };
        case GET_ALL_MATERIALS_SUCCESS:
            return {
                ...state,
                materials: action.payload,
                loading: false,
                success: action.success ?? true,
                errors: [],
            };
        case GET_ALL_MATERIAL_TYPES_SUCCESS:
            return {
                ...state,
                types: action.payload,
                loading: false,
                success: action.success ?? true,
                errors: [],
            };
        case GET_ALL_MATERIAL_THEMES_SUCCESS:
            return {
                ...state,
                themes: action.payload,
                loading: false,
                success: action.success ?? true,
                errors: [],
            };
        case DELETE_MATERIAL_SUCCESS:
            return {
                ...state,
                materials: state.materials.filter(theory => theory.id !== action.payload),
                loading: false,
                success: action.success ?? true,
                errors: [],
            };
        case EXTRACT_VIDEO_ID_BY_URL_SUCCESS:
            return {
                ...state,
                videoId: action.payload,
                loading: false,
                success: action.success ?? true,
                errors: [],
            };
        case GET_ALL_MATERIALS_FAILURE:
        case GET_ALL_MATERIAL_TYPES_FAILURE:
        case GET_ALL_MATERIAL_THEMES_FAILURE:
        case EXTRACT_VIDEO_ID_BY_URL_FAILURE:
            return {
                ...state,
                loading: false,
                success: false,
                errors: action.payload || [],
                videoId: null,
            };
        default:
            return state;
    }
};