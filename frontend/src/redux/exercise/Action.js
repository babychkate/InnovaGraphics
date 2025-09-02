import { baseUrl } from "@/config/constants";
import { GET_ALL_EXERCISES_FAILURE, GET_ALL_EXERCISES_REQUEST, GET_ALL_EXERCISES_SUCCESS, GET_EXERCISE_BY_PLANET_ID_FAILURE, GET_EXERCISE_BY_PLANET_ID_REQUEST, GET_EXERCISE_BY_PLANET_ID_SUCCESS } from "./ActionType"
import axios from "axios";

export const getExerciseByPlanetId = (id) => async (dispatch) => {
    dispatch({ type: GET_EXERCISE_BY_PLANET_ID_REQUEST });

    try {
        const response = await axios.get(`${baseUrl}/api/Exercise/get-by-planet-id/${id}`, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        dispatch({ type: GET_EXERCISE_BY_PLANET_ID_SUCCESS, payload: response.data });
    } catch (e) {
        console.log(e);
        dispatch({ type: GET_EXERCISE_BY_PLANET_ID_FAILURE });
    }
}

export const getAllExercises = () => async (dispatch) => {
    dispatch({ type: GET_ALL_EXERCISES_REQUEST });

    try {
        const response = await axios.get(`${baseUrl}/api/Exercise/get-all-exercises`, {
            headers: {
                'Content-Type': 'application/json'
            },
            withCredentials: true,
        });
        dispatch({ type: GET_ALL_EXERCISES_SUCCESS, payload: response.data });
    } catch (e) {
        console.log(e);
        dispatch({ type: GET_ALL_EXERCISES_FAILURE });
    }
}