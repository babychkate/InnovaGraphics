import {
    CREATE_TEST_REQUEST,
    CREATE_TEST_SUCCESS,
    CREATE_TEST_FAILURE,
    CLEAR_ERRORS_REQUEST,
    GET_ALL_TESTS_REQUEST,
    GET_ALL_TESTS_SUCCESS,
    GET_ALL_TESTS_FAILURE,
    UPDATE_TEST_REQUEST,
    UPDATE_TEST_FAILURE,
    UPDATE_TEST_SUCCESS,
    DELETE_TEST_SUCCESS,
    GET_TEST_WITH_QUESTIONS_REQUEST,
    GET_TEST_WITH_QUESTIONS_SUCCESS,
    GET_TEST_WITH_QUESTIONS_FAILURE,
    START_TEST_REQUEST,
    START_TEST_SUCCESS,
    START_TEST_FAILURE,
    COMPLETE_TEST_REQUEST,
    COMPLETE_TEST_SUCCESS,
    COMPLETE_TEST_FAILURE,
    SET_TEST_AFTER_ACCEPT_INVITATION,
    GET_TEST_BY_PLANET_ID_REQUEST,
    GET_TEST_BY_PLANET_ID_SUCCESS,
    GET_TEST_BY_PLANET_ID_FAILURE,
    GET_TEST_BY_SUBTOPIC_REQUEST,
    GET_TEST_BY_SUBTOPIC_SUCCESS,
    GET_TEST_BY_SUBTOPIC_FAILURE,
    SET_BATTLE_ID,
    SET_SELECTED_TEST,
    RESET_SELECTED_TEST,
} from "./ActionType";

const initialState = {
    test: null,
    tests: [],
    questions: [],
    battleId: null,
    selectedTest: null,
    loading: false,
    success: false,
    errors: [],
};

export const testReducer = (state = initialState, action) => {
    switch (action.type) {
        case CREATE_TEST_REQUEST:
        case GET_ALL_TESTS_REQUEST:
        case UPDATE_TEST_REQUEST:
        case GET_TEST_WITH_QUESTIONS_REQUEST:
        case START_TEST_REQUEST:
        case COMPLETE_TEST_REQUEST:
        case GET_TEST_BY_PLANET_ID_REQUEST:
        case GET_TEST_BY_SUBTOPIC_REQUEST:
            return {
                ...state,
                loading: true,
                success: false,
                errors: [],
            };
        case CREATE_TEST_SUCCESS:
        case START_TEST_SUCCESS:
        case COMPLETE_TEST_SUCCESS:
        case SET_TEST_AFTER_ACCEPT_INVITATION:
        case GET_TEST_BY_PLANET_ID_SUCCESS:
        case GET_TEST_BY_SUBTOPIC_SUCCESS:
            return {
                ...state,
                test: action.payload,
                loading: false,
                success: true,
                errors: [],
            };
        case GET_ALL_TESTS_SUCCESS:
            return {
                ...state,
                tests: action.payload,
                loading: false,
                success: action.success ?? true,
                errors: [],
            };
        case DELETE_TEST_SUCCESS:
            return {
                ...state,
                tests: state.tests.filter(test => test.id !== action.payload),
            };
        case UPDATE_TEST_SUCCESS:
            return {
                ...state,
                loading: false,
                success: true,
            };
        case GET_TEST_WITH_QUESTIONS_SUCCESS:
            return {
                ...state,
                questions: action.payload,
                loading: false,
                success: true,
            };
        case SET_BATTLE_ID:
            return {
                ...state,
                battleId: action.payload,
            };
        case SET_SELECTED_TEST:
        case RESET_SELECTED_TEST:
            return {
                ...state,
                selectedTest: action.payload,
            };
        case CREATE_TEST_FAILURE:
        case GET_ALL_TESTS_FAILURE:
        case UPDATE_TEST_FAILURE:
        case START_TEST_FAILURE:
        case COMPLETE_TEST_FAILURE:
        case GET_TEST_WITH_QUESTIONS_FAILURE:
        case GET_TEST_BY_PLANET_ID_FAILURE:
        case GET_TEST_BY_SUBTOPIC_FAILURE:
            return {
                ...state,
                test: null,
                loading: false,
                success: false,
                errors: action.payload || [],
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