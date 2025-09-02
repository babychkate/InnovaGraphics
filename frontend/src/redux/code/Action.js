import { baseUrl } from "@/config/constants";
import axios from "axios";
import { toast } from "react-toastify";
import { CLEAR_OUTPUT, RUN_CODE_FAILURE, RUN_CODE_REQUEST, RUN_CODE_SUCCESS, RUN_LAB1_CODE_FAILURE, RUN_LAB1_CODE_REQUEST, RUN_LAB1_CODE_SUCCESS, RUN_LAB3_CODE_FAILURE, RUN_LAB3_CODE_REQUEST, RUN_LAB3_CODE_SUCCESS, RUN_LAB4_CODE_FAILURE, RUN_LAB4_CODE_REQUEST, RUN_LAB4_CODE_SUCCESS, RUN_LAB5_CODE_FAILURE, RUN_LAB5_CODE_REQUEST, RUN_LAB5_CODE_SUCCESS, TEST_CODE_FAILURE, TEST_CODE_REQUEST, TEST_CODE_SUCCESS } from "./ActionType";

export const runCode = (data) => async (dispatch) => {
    dispatch({ type: RUN_CODE_REQUEST });

    try {
        const response = await axios.post(`${baseUrl}/api/Code/run`, data, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        console.log(response);
        dispatch({ type: RUN_CODE_SUCCESS, payload: response?.data?.curvePoints, outputText: response?.data?.outputText, success: response.data.success });
    } catch (e) {
        console.log(e);
        dispatch({ type: RUN_CODE_FAILURE, payload: e.response?.data?.validationErrors });
        toast.error(e.response?.data);
    }
};

export const runLab1Code = (data) => async (dispatch) => {
    dispatch({ type: RUN_LAB1_CODE_REQUEST });

    try {
        const response = await axios.post(`${baseUrl}/api/Code/run1`, data, {
            headers: {
                "Content-Type": "application/json",
            },
            withCredentials: true,
        });

        dispatch({
            type: RUN_LAB1_CODE_SUCCESS,
            payload: response.data?.curvePoints,
            outputText: response?.data?.outputText,
            success: true,
        });
    } catch (e) {
        dispatch({
            type: RUN_LAB1_CODE_FAILURE,
            payload: e.response?.data?.validationErrors || "Помилка",
        });

        toast.error(e.response?.data?.message || "Помилка при виконанні ЛР1");
    }
};

export const runLab3Code = (data) => async (dispatch) => {
    dispatch({ type: RUN_LAB3_CODE_REQUEST });

    try {
        const response = await axios.post(`${baseUrl}/api/Code/run3`, data, {
            headers: {
                "Content-Type": "application/json",
            },
            withCredentials: true,
        });
        console.log(response);
        dispatch({
            type: RUN_LAB3_CODE_SUCCESS,
            payload: response.data?.curvePoints,
            outputText: response?.data?.outputText,
            success: true,
        });
    } catch (e) {
        dispatch({
            type: RUN_LAB3_CODE_FAILURE,
            payload: e.response?.data?.validationErrors || "Помилка",
        });

        toast.error(e.response?.data?.message || "Помилка при виконанні ЛР3");
    }
};

export const runLab4Code = (data) => async (dispatch) => {
    dispatch({ type: RUN_LAB4_CODE_REQUEST });

    try {
        const response = await axios.post(`${baseUrl}/api/Code/run4`, data, {
            headers: {
                "Content-Type": "application/json",
            },
            withCredentials: true,
        });

        dispatch({
            type: RUN_LAB4_CODE_SUCCESS,
            payload: response.data.convertedImageBase64,
            success: true,
        });
    } catch (e) {
        dispatch({
            type: RUN_LAB4_CODE_FAILURE,
            payload: e.response?.data?.validationErrors || "Помилка",
        });

        toast.error(e.response?.data?.message || "Помилка при виконанні ЛР4");
    }
};

export const runLab5Code = (data) => async (dispatch) => {
    dispatch({ type: RUN_LAB5_CODE_REQUEST });

    try {
        const response = await axios.post(`${baseUrl}/api/Code/run5`, data, {
            headers: {
                "Content-Type": "application/json",
            },
            withCredentials: true,
        });
        console.log(response);
        dispatch({
            type: RUN_LAB5_CODE_SUCCESS,
            expectedPoints: response.data?.expectedPoints,
            realPoints: response.data?.realPoints,
            outputText: response?.data?.outputText,
            success: true,
        });
    } catch (e) {
        console.log(e);
        dispatch({
            type: RUN_LAB5_CODE_FAILURE,
            payload: e.response?.data?.validationErrors || "Помилка",
        });

        toast.error(e.response?.data?.message || "Помилка при виконанні ЛР5");
    }
};

export const testCode = (data) => async (dispatch) => {
    dispatch({ type: TEST_CODE_REQUEST });

    try {
        const response = await axios.post(`${baseUrl}/api/Code/test`, data, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        console.log(response);
        dispatch({ type: TEST_CODE_SUCCESS, payload: response.data, success: response.data.success });
    } catch (e) {
        console.log(e);
        dispatch({ type: TEST_CODE_FAILURE, payload: e.response?.data?.validationErrors });
        toast.error(e.response?.data?.message);
    }
};

export const clearOutput = () => (dispatch) => {
    dispatch({ type: CLEAR_OUTPUT });
};