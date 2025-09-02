import { toast } from "react-toastify";
import { CLEAR_ERRORS_REQUEST, CREATE_PLANET_FAILURE, CREATE_PLANET_REQUEST, CREATE_PLANET_SUCCESS, DELETE_PLANET_FAILURE, DELETE_PLANET_REQUEST, DELETE_PLANET_SUCCESS, GET_ALL_PLANET_SUBTOPICS_FAILURE, GET_ALL_PLANET_SUBTOPICS_REQUEST, GET_ALL_PLANET_SUBTOPICS_SUCCESS, GET_ALL_PLANET_TOPICS_FAILURE, GET_ALL_PLANET_TOPICS_REQUEST, GET_ALL_PLANET_TOPICS_SUCCESS, GET_ALL_PLANETS_FAILURE, GET_ALL_PLANETS_REQUEST, GET_ALL_PLANETS_SUCCESS, GET_PLANET_FAILURE, GET_PLANET_REQUEST, GET_PLANET_SUCCESS, UPDATE_PLANET_FAILURE, UPDATE_PLANET_REQUEST, UPDATE_PLANET_SUCCESS } from "./ActionType";
import axios from "axios";
import { baseUrl } from "@/config/constants";

export const createPlanet = (data) => async (dispatch) => {
    dispatch({ type: CREATE_PLANET_REQUEST });

    try {
        const response = await axios.post(`${baseUrl}/api/Planet/create-planet`, data, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        dispatch({ type: CREATE_PLANET_SUCCESS, payload: data, success: response?.data?.success });
        toast.success(response?.data?.message);
        return { success: response?.data?.success }; 
    } catch (e) {
        console.log(e?.response?.data);
        dispatch({ type: CREATE_PLANET_FAILURE, payload: e.response?.data?.validationErrors });
        toast.error(e.response?.data?.message);
        return { success: response?.data?.success }; 
    }
};

export const getAllPlanets = () => async (dispatch) => {
    dispatch({ type: GET_ALL_PLANETS_REQUEST });

    try {
        const response = await axios.get(`${baseUrl}/api/Planet/get-all-planets`, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        dispatch({ type: GET_ALL_PLANETS_SUCCESS, payload: response?.data, success: response?.data?.success });
        toast.success(response?.data?.message);
    } catch (e) {
        console.log(e);
        dispatch({ type: GET_ALL_PLANETS_FAILURE, payload: e.response?.data?.validationErrors });
        toast.error(e.response?.data?.message);
    }
}

export const getPlanetById = (id) => async (dispatch) => {
    dispatch({ type: GET_PLANET_REQUEST });

    try {
        const response = await axios.get(`${baseUrl}/api/Planet/get-planet-with-id/${id}`, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        dispatch({ type: GET_PLANET_SUCCESS, payload: response?.data, success: response?.data?.success });
    } catch (e) {
        dispatch({ type: GET_PLANET_FAILURE, payload: e.response?.data?.validationErrors });
    }
}

export const deletePlanet = (id) => async (dispatch) => {
    dispatch({ type: DELETE_PLANET_REQUEST });

    try {
        const response = await axios.delete(`${baseUrl}/api/Planet/delete-planet/${id}`, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        dispatch({ type: DELETE_PLANET_SUCCESS, payload: id });
        toast.success(response?.data?.message);
    } catch (e) {
        dispatch({ type: DELETE_PLANET_FAILURE });
        toast.error(e?.response?.data?.message || "Невдалося видалити планету!");
    }
}

export const updatePlanet = (id, patches) => async (dispatch) => {
    dispatch({ type: UPDATE_PLANET_REQUEST });

    try {
        const response = await axios.patch(`${baseUrl}/api/Planet/update-planet/${id}`, patches, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        dispatch({ type: UPDATE_PLANET_SUCCESS, payload: id });
        toast.success("Планета оновлена успішно!");
        return { success: true };
    } catch (e) {
        console.log(e);
        dispatch({ type: UPDATE_PLANET_FAILURE });
        toast.error(e?.response?.data?.message || "Невдалося оновити планету!");
        return { success: false };
    }
}

export const clearPlanetErrors = () => ({
    type: CLEAR_ERRORS_REQUEST,
});


export const getAllPlanetTopics = () => async (dispatch) => {
    dispatch({ type: GET_ALL_PLANET_TOPICS_REQUEST });

    try {
        const response = await axios.get(`${baseUrl}/api/Planet/get-all-planet-topics`, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        console.log(response.data);
        dispatch({ type: GET_ALL_PLANET_TOPICS_SUCCESS, payload: response?.data });
    } catch (e) {;
        dispatch({ type: GET_ALL_PLANET_TOPICS_FAILURE });
    }
}

export const getAllPlanetSubTopics = () => async (dispatch) => {
    dispatch({ type: GET_ALL_PLANET_SUBTOPICS_REQUEST });

    try {
        const response = await axios.get(`${baseUrl}/api/Planet/get-all-planet-subtopics`, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        dispatch({ type: GET_ALL_PLANET_SUBTOPICS_SUCCESS, payload: response?.data });
    } catch (e) {
        dispatch({ type: GET_ALL_PLANET_SUBTOPICS_FAILURE });
    }
}