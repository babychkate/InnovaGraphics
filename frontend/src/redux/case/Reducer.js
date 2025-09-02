import { CREATE_CASE_FAILURE, CREATE_CASE_REQUEST, CREATE_CASE_SUCCESS, DELETE_CASE_FAILURE, DELETE_CASE_REQUEST, DELETE_CASE_SUCCESS, GET_ALL_CASES_FAILURE, GET_ALL_CASES_REQUEST, GET_ALL_CASES_SUCCESS, UPDATE_CASE_FAILURE, UPDATE_CASE_REQUEST } from "./ActionType";

const initialState = {
    case: null,
    cases: [],
    loading: false,
    success: false,
    errors: [],
};

export const caseReducer = (state = initialState, action) => {
    switch (action.type) {
        case CREATE_CASE_REQUEST:
        case GET_ALL_CASES_REQUEST:
        case UPDATE_CASE_REQUEST:
        case DELETE_CASE_REQUEST:
            return {
                ...state,
                loading: true,
                success: false,
                errors: [],
            };
        case CREATE_CASE_SUCCESS:
            return {
                ...state,
                case: action.payload,
                loading: false,
                success: action.success ?? true,
                errors: [],
            };
        case GET_ALL_CASES_SUCCESS:
            return {
                ...state,
                cases: action.payload,
                loading: false,
                success: action.success ?? true,
                errors: [],
            };
        case DELETE_CASE_SUCCESS:
            return {
                ...state,
                cases: state.cases.filter(c => c.id !== action.payload),
                loading: false,
                success: action.success ?? true,
                errors: [],
            };
        case CREATE_CASE_FAILURE:
        case GET_ALL_CASES_FAILURE:
        case UPDATE_CASE_FAILURE:
        case DELETE_CASE_FAILURE:
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