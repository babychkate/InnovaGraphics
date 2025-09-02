import { toast } from "react-toastify";
import { ADD_QUESTION_ANSWERS_FAILURE, ADD_QUESTION_ANSWERS_REQUEST, ADD_QUESTION_ANSWERS_SUCCESS, CLEAR_ERRORS_REQUEST, COMPLETE_TEST_FAILURE, COMPLETE_TEST_REQUEST, COMPLETE_TEST_SUCCESS, CREATE_TEST_FAILURE, CREATE_TEST_REQUEST, CREATE_TEST_SUCCESS, DELETE_QUESTION_FAILURE, DELETE_QUESTION_REQUEST, DELETE_QUESTION_SUCCESS, DELETE_TEST_FAILURE, DELETE_TEST_REQUEST, DELETE_TEST_SUCCESS, GET_ALL_TESTS_FAILURE, GET_ALL_TESTS_REQUEST, GET_ALL_TESTS_SUCCESS, GET_TEST_BY_PLANET_ID_FAILURE, GET_TEST_BY_PLANET_ID_REQUEST, GET_TEST_BY_PLANET_ID_SUCCESS, GET_TEST_WITH_QUESTIONS_FAILURE, GET_TEST_WITH_QUESTIONS_REQUEST, GET_TEST_WITH_QUESTIONS_SUCCESS, RESET_SELECTED_TEST, SET_BATTLE_ID, SET_SELECTED_TEST, SET_TEST_AFTER_ACCEPT_INVITATION, START_TEST_FAILURE, START_TEST_REQUEST, START_TEST_SUCCESS, UPDATE_TEST_FAILURE, UPDATE_TEST_REQUEST, UPDATE_TEST_SUCCESS } from "./ActionType";
import axios from "axios";
import { baseUrl } from "@/config/constants";
import test from "node:test";

export const createTest = (data) => async (dispatch) => {
    dispatch({ type: CREATE_TEST_REQUEST });

    try {
        const response = await axios.post(`${baseUrl}/api/Test/create-test`, data, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        console.log(response);
        dispatch({ type: CREATE_TEST_SUCCESS, payload: data, success: response?.data?.success });
        toast.success(response?.data);
        return { success: true };
    } catch (e) {
        console.log(e);
        dispatch({ type: CREATE_TEST_FAILURE, payload: e.response?.data?.validationErrors });
        toast.error(e.response?.data?.message);
        return { success: false };
    }
}

export const getAllTests = () => async (dispatch) => {
    dispatch({ type: GET_ALL_TESTS_REQUEST });

    try {
        const response = await axios.get(`${baseUrl}/api/Test/get-all-tests`, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        dispatch({ type: GET_ALL_TESTS_SUCCESS, payload: response?.data, success: response?.data?.success });
        toast.success(response?.data?.message);
    } catch (e) {
        dispatch({ type: GET_ALL_TESTS_FAILURE, payload: e.response?.data?.validationErrors });
        toast.error(e.response?.data?.message);
    }
}

export const getTestByPlanetId = (id) => async (dispatch) => {
    dispatch({ type: GET_TEST_BY_PLANET_ID_REQUEST });

    try {
        const response = await axios.get(`${baseUrl}/api/Test/get-test-by-planet-id/${id}`, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        dispatch({ type: GET_TEST_BY_PLANET_ID_SUCCESS, payload: response?.data, success: response?.data?.success });
    } catch (e) {
        dispatch({ type: GET_TEST_BY_PLANET_ID_FAILURE, payload: e.response?.data?.validationErrors });
    }
}

export const getTestBySubtopic = (subtopic) => async (dispatch) => {
    dispatch({ type: GET_TEST_BY_PLANET_ID_REQUEST });

    try {
        const response = await axios.get(`${baseUrl}/api/Test/get-test-by-subtopic/${subtopic}`, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        dispatch({ type: GET_TEST_BY_PLANET_ID_SUCCESS, payload: response?.data, success: response?.data?.success });
    } catch (e) {
        dispatch({ type: GET_TEST_BY_PLANET_ID_FAILURE, payload: e.response?.data?.validationErrors });
    }
}

export const updateTest = (id, patches) => async (dispatch) => {
    dispatch({ type: UPDATE_TEST_REQUEST });

    try {
        const response = await axios.patch(`${baseUrl}/api/Test/update-test/${id}`, patches, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        dispatch({ type: UPDATE_TEST_SUCCESS, payload: id });
        toast.success("Тест оновлена успішно!");
        return { success: true };
    } catch (e) {
        console.log(e);
        dispatch({ type: UPDATE_TEST_FAILURE });
        toast.error(e?.response?.data?.message || "Невдалося оновити тест!");
        return { success: false };
    }
}

export const deleteTest = (id) => async (dispatch) => {
    dispatch({ type: DELETE_TEST_REQUEST });

    try {
        const response = await axios.delete(`${baseUrl}/api/Test/delete-test/${id}`, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        dispatch({ type: DELETE_TEST_SUCCESS, payload: id });
        toast.success(response?.data?.message || "Тест видалено успішно!");
    } catch (e) {
        console.log(e);
        dispatch({ type: DELETE_TEST_FAILURE });
        toast.error(e?.response?.data?.message || "Невдалося видалити тест!");
    }
}

export const addQuestionAnswers = (testId, data) => async (dispatch) => {
    dispatch({ type: ADD_QUESTION_ANSWERS_REQUEST });

    try {
        const response = await axios.post(`${baseUrl}/api/Test/tests/${testId}/add-questions-answers`, data, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        console.log(response);
        dispatch({ type: ADD_QUESTION_ANSWERS_SUCCESS, payload: data, success: response?.data?.success });
        toast.success(response?.data?.message);
    } catch (e) {
        console.log(e);
        dispatch({ type: ADD_QUESTION_ANSWERS_FAILURE, payload: e.response?.data?.validationErrors });
        toast.error(e.response?.data?.message);
    }
}

export const getTestWithQuestions = (id) => async (dispatch) => {
    dispatch({ type: GET_TEST_WITH_QUESTIONS_REQUEST });

    try {
        const response = await axios.get(`${baseUrl}/api/Test/get-test-with-questions-answers/${id}`, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        console.log(response);
        dispatch({ type: GET_TEST_WITH_QUESTIONS_SUCCESS, payload: response.data?.questions });
        toast.success(response?.data?.message);
    } catch (e) {
        console.log(e);
        dispatch({ type: GET_TEST_WITH_QUESTIONS_FAILURE, payload: e.response?.data?.validationErrors });
        toast.error(e.response?.data?.message);
    }
}

export const deleteQuestionFromTest = (id) => async (dispatch) => {
    dispatch({ type: DELETE_QUESTION_REQUEST });

    try {
        const response = await axios.delete(`${baseUrl}/api/Question/delete-question/${id}`, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        console.log(response);
        dispatch({ type: DELETE_QUESTION_SUCCESS, payload: response.data?.questions });
        toast.success(response?.data?.message);
    } catch (e) {
        console.log(e);
        dispatch({ type: DELETE_QUESTION_FAILURE, payload: e.response?.data?.validationErrors });
        toast.error(e.response?.data?.message);
    }
}

export const startTest = (testId, data) => async (dispatch) => {
    dispatch({ type: START_TEST_REQUEST });

    try {
        const response = await axios.post(`${baseUrl}/api/Test/start-test/${testId}`, data, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        console.log(response);
        dispatch({ type: START_TEST_SUCCESS, payload: response.data });
    } catch (e) {
        console.log(e);
        dispatch({ type: START_TEST_FAILURE, payload: e.response?.data?.validationErrors });
    }
}

export const completeTest = (testId, data) => async (dispatch) => {
    dispatch({ type: COMPLETE_TEST_REQUEST });
    console.log("Complete test action called with testId:", testId, "and data:", data);
    try {
        const response = await axios.post(`${baseUrl}/api/Test/complete-test/${testId}`, data, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        console.log(response);
        dispatch({ type: COMPLETE_TEST_SUCCESS, payload: response.data });
        toast.success(response?.data?.message);
    } catch (e) {
        console.log(e);
        dispatch({ type: COMPLETE_TEST_FAILURE, payload: e.response?.data?.validationErrors });
        toast.error(e.response?.data?.message);
    }
}

export const clearPlanetErrors = () => ({
    type: CLEAR_ERRORS_REQUEST,
});

export const setTestForBattle = (test) => async (dispatch) => {
    dispatch({ type: SET_TEST_AFTER_ACCEPT_INVITATION, paload: test });
}

export const setBattleId = (battleId) => async (dispatch) => {
    dispatch({ type: SET_BATTLE_ID, payload: battleId });
}

export const setSelectedTest = (test) => async (dispatch) => {
    dispatch({ type: SET_SELECTED_TEST, payload: test });
}

export const resetSelectedTest = () => async (dispatch) => {
    dispatch({ type: RESET_SELECTED_TEST, payload: null });
}