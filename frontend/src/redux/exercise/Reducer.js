import { GET_ALL_EXERCISES_FAILURE, GET_ALL_EXERCISES_REQUEST, GET_ALL_EXERCISES_SUCCESS, GET_EXERCISE_BY_PLANET_ID_FAILURE, GET_EXERCISE_BY_PLANET_ID_REQUEST, GET_EXERCISE_BY_PLANET_ID_SUCCESS } from "./ActionType";

const initialState = {
    exercise: null,
    exercises: [],
    loading: false,
    success: false,
    errors: [],
};

export const exerciseReducer = (state = initialState, action) => {
    switch (action.type) {
        case GET_EXERCISE_BY_PLANET_ID_REQUEST:
        case GET_ALL_EXERCISES_REQUEST:
            return {
                ...state,
                loading: true,
                success: false,
                errors: [],
            };
        case GET_EXERCISE_BY_PLANET_ID_SUCCESS:
        case GET_ALL_EXERCISES_SUCCESS:
            return {
                ...state,
                exercises: action.payload,
                loading: false,
                success: action.success ?? true,
                errors: [],
            };
        case GET_EXERCISE_BY_PLANET_ID_FAILURE:
        case GET_ALL_EXERCISES_FAILURE:
            return {
                ...state,
                loading: false,
                success: false,
                errors: action.payload || [],
                exercises: [],
            };
        default:
            return state;
    }
};