import { CREATE_ANSWER_FAILURE } from "../answer/ActionType";
import { CREATE_AVATAR_REQUEST, CREATE_AVATAR_SUCCESS, DELETE_AVATAR_FAILURE, DELETE_AVATAR_REQUEST, DELETE_AVATAR_SUCCESS, GET_ALL_AVATARS_FAILURE, GET_ALL_AVATARS_REQUEST, GET_ALL_AVATARS_SUCCESS, GET_USER_PURCHASES_FAILURE, GET_USER_PURCHASES_REQUEST, GET_USER_PURCHASES_SUCCESS, UPDATE_AVATAR_FAILURE, UPDATE_AVATAR_REQUEST, UPDATE_AVATAR_SUCCESS } from "./ActionType";

const initialState = {
    shopItems: [],
    shopItem: [],
    loading: false,
    success: false,
    errors: [],
};

export const shopItemsReducer = (state = initialState, action) => {
    switch (action.type) {
        case GET_ALL_AVATARS_REQUEST:
        case CREATE_AVATAR_REQUEST:
        case UPDATE_AVATAR_REQUEST:
        case DELETE_AVATAR_REQUEST:
        case GET_USER_PURCHASES_REQUEST:
            return {
                ...state,
                loading: true,
                success: false,
                errors: [],
            };
        case CREATE_AVATAR_SUCCESS:
            return {
                ...state,
                shopItem: action.payload,
                loading: false,
                success: true,
                errors: [],
            };
        case GET_ALL_AVATARS_SUCCESS:
        case GET_USER_PURCHASES_SUCCESS:
            return {
                ...state,
                shopItems: action.payload,
                loading: false,
                success: action.success ?? true,
                errors: [],
            };
        case DELETE_AVATAR_SUCCESS:
            return {
                ...state,
                shopItems: state.shopItems.filter(test => test.id !== action.payload),
            };
        case UPDATE_AVATAR_SUCCESS:
            return {
                ...state,
                loading: false,
                success: true,
            };
        case CREATE_ANSWER_FAILURE:
        case GET_ALL_AVATARS_FAILURE:
        case UPDATE_AVATAR_FAILURE:
        case DELETE_AVATAR_FAILURE:
        case GET_USER_PURCHASES_FAILURE:
            return {
                ...state,
                loading: false,
                success: false,
                shopItems: [],
                errors: action.payload || [],
            };
        default:
            return state;
    }
};