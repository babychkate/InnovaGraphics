import axios from "axios";
import { baseUrl } from "@/config/constants";
import { CREATE_THEORY_FAILURE, CREATE_THEORY_REQUEST, CREATE_THEORY_SUCCESS, DELETE_THEORY_FAILURE, DELETE_THEORY_REQUEST, DELETE_THEORY_SUCCESS, GET_ALL_THEORIES_FAILURE, GET_ALL_THEORIES_REQUEST, GET_ALL_THEORIES_SUCCESS, GET_THEORY_BY_ID_FAILURE, GET_THEORY_BY_ID_REQUEST, GET_THEORY_BY_ID_SUCCESS, GET_THEORY_BY_PLANET_ID_FAILURE, GET_THEORY_BY_PLANET_ID_REQUEST, GET_THEORY_BY_PLANET_ID_SUCCESS, UPDATE_THEORY_FAILURE, UPDATE_THEORY_REQUEST, UPDATE_THEORY_SUCCESS } from "./ActionType";

export const getAllTheories = () => async (dispatch) => {
    dispatch({ type: GET_ALL_THEORIES_REQUEST });

    try {
        const response = await axios.get(`${baseUrl}/api/Theory/get-all-theories`, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        console.log(response);
        dispatch({ type: GET_ALL_THEORIES_SUCCESS, payload: response.data, success: response?.data?.success });
    } catch (e) {
        console.log(e);
        dispatch({ type: GET_ALL_THEORIES_FAILURE, payload: e.response?.data?.validationErrors });
    }
}

export const getTheoryById = (id) => async (dispatch) => {
    dispatch({ type: GET_THEORY_BY_ID_REQUEST });

    try {
        const response = await axios.get(`${baseUrl}/api/Theory/get-theory-with-id/${id}`, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        console.log(response);
        dispatch({ type: GET_THEORY_BY_ID_SUCCESS, payload: response.data.content, success: response?.data?.success });
    } catch (e) {
        console.log(e);
        dispatch({ type: GET_THEORY_BY_ID_FAILURE, payload: e.response?.data?.validationErrors });
    }
}

export const getTheoryByPlanetId = (id) => async (dispatch) => {
    dispatch({ type: GET_THEORY_BY_PLANET_ID_REQUEST });

    try {
        const response = await axios.get(`${baseUrl}/api/Theory/get-by-planet-id/${id}`, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        dispatch({ type: GET_THEORY_BY_PLANET_ID_SUCCESS, payload: response.data?.[0]?.content, success: response?.data?.success });
    } catch (e) {
        console.log(e);
        dispatch({ type: GET_THEORY_BY_PLANET_ID_FAILURE, payload: e.response?.data?.validationErrors });
    }
}

export const createTheory = (data) => async (dispatch) => {
    dispatch({ type: CREATE_THEORY_REQUEST });

    try {
        const response = await axios.post(`${baseUrl}/api/Theory/create-theory`, data, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        console.log(response);
        dispatch({ type: CREATE_THEORY_SUCCESS, payload: response.data });
        return { success: true };
    } catch (e) {
        console.log(e);
        dispatch({ type: CREATE_THEORY_FAILURE, payload: e.response?.data?.validationErrors });
        return { success: false };
    }
}

export const updateTheory = (id, patches) => async (dispatch) => {
    dispatch({ type: UPDATE_THEORY_REQUEST });
    console.log(patches);
    try {
        const response = await axios.patch(`${baseUrl}/api/Theory/update-theory/${id}`, patches, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        console.log(response);
        dispatch({ type: UPDATE_THEORY_SUCCESS, payload: response.data });
        return { success: true };
    } catch (e) {
        console.log(e);
        dispatch({ type: UPDATE_THEORY_FAILURE, payload: e.response?.data?.validationErrors });
        return { success: false };
    }
}

export const deleteTheory = (id) => async (dispatch) => {
    dispatch({ type: DELETE_THEORY_REQUEST });

    try {
        const response = await axios.delete(`${baseUrl}/api/Theory/delete-theory/${id}`, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        console.log(response);
        dispatch({ type: DELETE_THEORY_SUCCESS, payload: id });
    } catch (e) {
        console.log(e);
        dispatch({ type: DELETE_THEORY_FAILURE, payload: e.response?.data?.validationErrors });
    }
}