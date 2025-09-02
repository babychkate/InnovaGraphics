import { DELETE_THEORY_REQUEST, DELETE_THEORY_SUCCESS, GET_ALL_THEORIES_FAILURE, GET_ALL_THEORIES_REQUEST, GET_ALL_THEORIES_SUCCESS, GET_THEORY_BY_ID_FAILURE, GET_THEORY_BY_ID_REQUEST, GET_THEORY_BY_ID_SUCCESS, GET_THEORY_BY_PLANET_ID_REQUEST, GET_THEORY_BY_PLANET_ID_SUCCESS } from "./ActionType";

const initialState = {
    theory: null,
    theories: [],
    loading: false,
    success: false,
    errors: [],
};

export const theoryReducer = (state = initialState, action) => {
    switch (action.type) {
        case GET_ALL_THEORIES_REQUEST:
        case GET_THEORY_BY_ID_REQUEST:
        case GET_THEORY_BY_PLANET_ID_REQUEST:
        case DELETE_THEORY_REQUEST:
            return {
                ...state,
                loading: true,
                success: false,
                errors: [],
            };
        case GET_ALL_THEORIES_SUCCESS:
            return {
                ...state,
                theories: action.payload,
                loading: false,
                success: action.success ?? true,
                errors: [],
            };
        case DELETE_THEORY_SUCCESS:
            return {
                ...state,
                theories: state.theories.filter(theory => theory.id !== action.payload),
                loading: false,
                success: action.success ?? true,
                errors: [],
            };
        case GET_ALL_THEORIES_FAILURE:
            return {
                ...state,
                loading: false,
                success: false,
                errors: action.payload || [],
            };
        case GET_THEORY_BY_ID_SUCCESS:
        case GET_THEORY_BY_PLANET_ID_SUCCESS:
            return {
                ...state,
                theory: action.payload,
                loading: false,
                success: action.success ?? true,
                errors: [],
            };
        default:
            return state;
    }
};