import {
    GET_CURRENT_USER_SUCCESS,
    LOGIN_SUCCESS,
    LOGOUT,
    REGISTER_FAILURE,
    REGISTER_REQUEST,
    REGISTER_SUCCESS,
    RESET_PASSWORD_FAILURE,
    RESET_PASSWORD_SUCCESS,
    VERIFY_FAILURE,
    VERIFY_REQUEST,
    VERIFY_SUCCESS
} from "./ActionType";

const initialState = {
    isAuthenticated: false,
    errors: [],
    user: null,
    loading: false,
    success: false,
    lastAction: null,
};

export const authReducer = (state = initialState, action) => {
    switch (action.type) {
        case REGISTER_REQUEST:
        case VERIFY_REQUEST:
            return {
                ...state,
                errors: [],
                isAuthenticated: false,
                loading: true,
                success: false,
                lastAction: null,
            };
        case REGISTER_SUCCESS:
            return {
                ...state,
                errors: [],
                isAuthenticated: false,
                user: action.payload,
                loading: false,
                success: true,
                lastAction: 'register',
            };
        case LOGIN_SUCCESS:
            return {
                ...state,
                errors: [],
                isAuthenticated: true,
                user: action.payload,
                loading: false,
                success: true,
                lastAction: 'login',
            };
        case VERIFY_SUCCESS:
            return {
                ...state,
                errors: [],
                isAuthenticated: true,
                loading: false,
                success: true,
                lastAction: 'verify',
            };
        case GET_CURRENT_USER_SUCCESS:
            return {
                ...state,
                errors: [],
                user: action.payload,
                isAuthenticated: true,
                loading: false,
                success: true,
                lastAction: 'getCurrentUser',
            };
        case LOGOUT:
            return {
                ...state,
                isAuthenticated: false,
                user: null,
                loading: false,
                success: false,
                lastAction: 'logout',
            };
        case REGISTER_FAILURE:
        case VERIFY_FAILURE:
            return {
                ...state,
                errors: action.payload,
                isAuthenticated: false,
                loading: false,
                success: false,
                lastAction: null,
            };
        case RESET_PASSWORD_SUCCESS:
            return {
                ...state,
                errors: [],
                loading: false,
                isAuthenticated: true,
                success: true,
                lastAction: 'resetPassword',
            };
        case RESET_PASSWORD_FAILURE:
            return {
                ...state,
                errors: action.payload || [],
                loading: false,
                isAuthenticated: true,
                success: false,
                lastAction: 'resetPassword',
            };
        default:
            return state;
    }
};