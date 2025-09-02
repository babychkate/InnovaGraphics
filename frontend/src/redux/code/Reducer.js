import { CLEAR_OUTPUT, RUN_CODE_FAILURE, RUN_CODE_REQUEST, RUN_CODE_SUCCESS, RUN_LAB1_CODE_FAILURE, RUN_LAB1_CODE_REQUEST, RUN_LAB1_CODE_SUCCESS, RUN_LAB3_CODE_FAILURE, RUN_LAB3_CODE_REQUEST, RUN_LAB3_CODE_SUCCESS, RUN_LAB4_CODE_FAILURE, RUN_LAB4_CODE_REQUEST, RUN_LAB4_CODE_SUCCESS, RUN_LAB5_CODE_FAILURE, RUN_LAB5_CODE_REQUEST, RUN_LAB5_CODE_SUCCESS, TEST_CODE_FAILURE, TEST_CODE_REQUEST, TEST_CODE_SUCCESS } from "./ActionType";

const initialState = {
    loading: false,
    success: false,
    points: [],
    outputText: '',
    errors: [],
    image: null,
    expectedPoints: [],
    realPoints: [],
    listErrors: [],
    caseResults: [],
};

export const codeReducer = (state = initialState, action) => {
    switch (action.type) {
        case RUN_CODE_REQUEST:
        case RUN_LAB4_CODE_REQUEST:
        case RUN_LAB1_CODE_REQUEST:
        case RUN_LAB3_CODE_REQUEST:
        case RUN_LAB5_CODE_REQUEST:
            return {
                ...state,
                errors: [],
                loading: true,
                success: false,
            };
        case RUN_CODE_SUCCESS:
        case RUN_LAB1_CODE_SUCCESS:
        case RUN_LAB3_CODE_SUCCESS:
            return {
                ...state,
                points: action.payload,
                loading: false,
                success: true,
                outputText: action.outputText,
            };
        case RUN_LAB4_CODE_SUCCESS:
            return {
                ...state,
                image: action.payload,
                loading: false,
                success: true,
                outputText: action.outputText,
            };
        case RUN_LAB5_CODE_SUCCESS:
            return {
                ...state,
                expectedPoints: action.expectedPoints,
                realPoints: action.realPoints,
                loading: false,
                success: true,
                outputText: action.outputText,
            };
        case RUN_CODE_FAILURE:
        case RUN_LAB1_CODE_FAILURE:
        case RUN_LAB3_CODE_FAILURE:
            return {
                ...state,
                points: [],
                loading: true,
                success: false,
                errors: action.payload
            };
        case RUN_LAB4_CODE_FAILURE:
            return {
                ...state,
                image: null,
                loading: true,
                success: false,
                errors: action.payload
            };
        case RUN_LAB5_CODE_FAILURE:
            return {
                ...state,
                expectedPoints: [],
                realPoints: [],
                loading: true,
                success: false,
                errors: action.payload
            };
        case CLEAR_OUTPUT:
            return {
                ...state,
                loading: true,
                success: false,
                outputText: '',
                points: [],
                expectedPoints: [],
                realPoints: [],
                image: null,
            };
        case TEST_CODE_REQUEST:
            return {
                ...state,
                loading: true,
                success: false,
                errors: [],
            };
        case TEST_CODE_SUCCESS:
            return {
                ...state,
                listErrors: action.payload.listErrors,
                caseResults: action.payload.caseResults,
                loading: false,
                success: action.success,
            };
        case TEST_CODE_FAILURE:
            return {
                ...state,
                listErrors: [],
                caseResults: [],
                loading: false,
                success: false,
            };
        default:
            return state;
    }
};