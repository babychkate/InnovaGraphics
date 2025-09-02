import { CLEAR_ERRORS_REQUEST, CREATE_PLANET_FAILURE, CREATE_PLANET_REQUEST, CREATE_PLANET_SUCCESS, DELETE_PLANET_SUCCESS, GET_ALL_PLANET_SUBTOPICS_FAILURE, GET_ALL_PLANET_SUBTOPICS_REQUEST, GET_ALL_PLANET_SUBTOPICS_SUCCESS, GET_ALL_PLANET_TOPICS_FAILURE, GET_ALL_PLANET_TOPICS_REQUEST, GET_ALL_PLANET_TOPICS_SUCCESS, GET_ALL_PLANETS_FAILURE, GET_ALL_PLANETS_REQUEST, GET_ALL_PLANETS_SUCCESS, GET_PLANET_FAILURE, GET_PLANET_REQUEST, GET_PLANET_SUCCESS, UPDATE_PLANET_FAILURE, UPDATE_PLANET_REQUEST, UPDATE_PLANET_SUCCESS } from "./ActionType";

const initialState = {
    planet: null,
    planets: [],
    loading: false,
    success: false,
    errors: [],
    topics: [],
    subTopics: [],
};

export const planetReducer = (state = initialState, action) => {
    switch (action.type) {
        case CREATE_PLANET_REQUEST:
        case GET_ALL_PLANETS_REQUEST:
        case UPDATE_PLANET_REQUEST:
        case GET_ALL_PLANET_TOPICS_REQUEST:
        case GET_ALL_PLANET_SUBTOPICS_REQUEST:
        case GET_PLANET_REQUEST:
            return {
                ...state,
                loading: true,
                success: false,
            };
        case CREATE_PLANET_SUCCESS:
        case GET_PLANET_SUCCESS:
            return {
                ...state,
                loading: false,
                success: true,
                planet: action.payload,
            };
        case GET_ALL_PLANETS_SUCCESS:
            return {
                ...state,
                loading: false,
                success: true,
                planets: action.payload,
            };
        case GET_ALL_PLANET_TOPICS_SUCCESS:
            return {
                ...state,
                loading: false,
                success: true,
                topics: action.payload,
            };
        case GET_ALL_PLANET_SUBTOPICS_SUCCESS:
            return {
                ...state,
                loading: false,
                success: true,
                subTopics: action.payload,
            };
        case DELETE_PLANET_SUCCESS:
            return {
                ...state,
                planets: state.planets.filter(planet => planet.id !== action.payload),
            };
        case UPDATE_PLANET_SUCCESS:
            return {
                ...state,
                loading: false,
                success: true,
            };
        case CREATE_PLANET_FAILURE:
        case GET_ALL_PLANETS_FAILURE:
        case UPDATE_PLANET_FAILURE:
        case GET_ALL_PLANET_TOPICS_FAILURE:
        case GET_ALL_PLANET_SUBTOPICS_FAILURE:
        case GET_PLANET_FAILURE:
            return {
                ...state,
                loading: false,
                success: false,
                errors: action.payload || {},
            };
        case CLEAR_ERRORS_REQUEST:
            return {
                ...state,
                errors: [],
            };
        default:
            return state;
    }
};