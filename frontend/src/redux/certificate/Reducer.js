import {
    GET_CERTIFICATE_FAILURE,
    GET_CERTIFICATE_REQUEST,
    GET_CERTIFICATE_SUCCESS,
} from "./ActionType";

const initialState = {
    certificate: null,
    loading: false,
    success: false,
    errors: [],
};

export const certificateReducer = (state = initialState, action) => {
    switch (action.type) {
        case GET_CERTIFICATE_REQUEST:
            return {
                ...state,
                loading: true,
                success: false,
                errors: [],
            };
        case GET_CERTIFICATE_SUCCESS:
            return {
                ...state,
                certificate: action.payload,
                loading: false,
                success: action.success ?? true,
                errors: null,
            };
        case GET_CERTIFICATE_FAILURE:
            return {
                ...state,
                loading: false,
                success: false,
                errors: action.payload || [],
            };
        default:
            return state;
    }
};