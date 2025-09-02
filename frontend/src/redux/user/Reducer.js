import { UPDATE_USER_PROFILE_FAILURE, UPDATE_USER_PROFILE_REQUEST, UPDATE_USER_PROFILE_SUCCESS } from "./ActionType";

const initialState = {
    user: null,
    loading: false,
    success: false,
    errors: [],
};

export const userReducer = (state = initialState, action) => {
    switch (action.type) {
        case UPDATE_USER_PROFILE_REQUEST:
            return {
                ...state,
                loading: true,
                success: false,
                errors: [],
            };
        case UPDATE_USER_PROFILE_SUCCESS:
            return {
                ...state,
                user: action.payload,
                loading: false,
                success: action.success ?? true,
                errors: [],
            };
        case UPDATE_USER_PROFILE_FAILURE:
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