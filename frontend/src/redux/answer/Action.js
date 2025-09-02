import { toast } from "react-toastify";
import axios from "axios";
import { baseUrl } from "@/config/constants";
import {
    CREATE_ANSWER_FAILURE,
    CREATE_ANSWER_REQUEST,
    CREATE_ANSWER_SUCCESS,
} from "./ActionType";

export const createAnswer = (questionId, data) => async (dispatch) => {
    dispatch({ type: CREATE_ANSWER_REQUEST });

    try {
        const response = await axios.post(
            `${baseUrl}/api/Answer/create-answer`,
            data,
            {
                headers: {
                    "Content-Type": "application/json-patch+json",
                },
                withCredentials: true,
                params: {
                    questionId: questionId, 
                },
            }
        );

        dispatch({
            type: CREATE_ANSWER_SUCCESS,
            payload: response.data,
            success: response?.data?.success,
        });
    } catch (e) {
        console.error(e);
        const errors = e.response?.data?.validationErrors;
        dispatch({ type: CREATE_ANSWER_FAILURE, payload: errors });
        toast.error(e.response?.data?.validationErrors[0]);
    }
};