import { baseUrl } from "@/config/constants";
import { CREATE_CASE_FAILURE, CREATE_CASE_REQUEST, CREATE_CASE_SUCCESS, DELETE_CASE_FAILURE, DELETE_CASE_REQUEST, DELETE_CASE_SUCCESS, GET_ALL_CASES_FAILURE, GET_ALL_CASES_REQUEST, GET_ALL_CASES_SUCCESS, UPDATE_CASE_FAILURE, UPDATE_CASE_REQUEST, UPDATE_CASE_SUCCESS } from "./ActionType";
import axios from "axios";

export const createCase = (data) => async (dispatch) => {
    dispatch({ type: CREATE_CASE_REQUEST });

    try {
        const response = await axios.post(`${baseUrl}/api/Case/create-case`, data, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        console.log(response);
        dispatch({ type: CREATE_CASE_SUCCESS, payload: response.data?.data });
        return { success: true };
    } catch (e) {
        console.log(e);
        dispatch({ type: CREATE_CASE_FAILURE });
        return { success: false };
    }
}

export const getAllCases = () => async (dispatch) => {
    dispatch({ type: GET_ALL_CASES_REQUEST });

    try {
        const response = await axios.get(`${baseUrl}/api/Case/get-all-cases`, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        console.log(response);
        dispatch({ type: GET_ALL_CASES_SUCCESS, payload: response.data, success: response?.data?.success });
    } catch (e) {
        console.log(e);
        dispatch({ type: GET_ALL_CASES_FAILURE, payload: e.response?.data?.validationErrors });
    }
}

export const updateCase = (id, patches) => async (dispatch) => {
    dispatch({ type: UPDATE_CASE_REQUEST });

    try {
        const response = await axios.patch(`${baseUrl}/api/Case/update-case/${id}`, patches, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        console.log(response);
        dispatch({ type: UPDATE_CASE_SUCCESS, payload: response.data });
        return { success: true };
    } catch (e) {
        console.log(e);
        dispatch({ type: UPDATE_CASE_FAILURE, payload: e.response?.data?.validationErrors });
        return { success: false };
    }
}

export const deleteCase= (id) => async (dispatch) => {
    dispatch({ type: DELETE_CASE_REQUEST });

    try {
        const response = await axios.delete(`${baseUrl}/api/Case/delete-case/${id}`, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        console.log(response);
        dispatch({ type: DELETE_CASE_SUCCESS, payload: id });
    } catch (e) {
        console.log(e);
        dispatch({ type: DELETE_CASE_FAILURE, payload: e.response?.data?.validationErrors });
    }
}

export const getAllCasesByExerciseId = (exerciseId) => async (dispatch) => {
    dispatch({ type: GET_ALL_CASES_REQUEST });

    try {
        const response = await axios.get(`${baseUrl}/api/Case/get-by-exercise-id/${exerciseId}`, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        console.log(response);
        dispatch({ type: GET_ALL_CASES_SUCCESS, payload: response.data, success: response?.data?.success });
    } catch (e) {
        console.log(e);
        dispatch({ type: GET_ALL_CASES_FAILURE, payload: e.response?.data?.validationErrors });
    }
}