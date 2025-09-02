import { toast } from "react-toastify";
import axios from "axios";
import { baseUrl } from "@/config/constants";
import {
    UPDATE_QUESTION_FAILURE,
    UPDATE_QUESTION_REQUEST,
    UPDATE_QUESTION_SUCCESS,
} from "./ActionType";

const generatePatchDocument = (original, updated) => {
    const patch = [];

    if (original.text !== updated.text) {
        patch.push({
            op: "replace",
            path: "/text",
            value: updated.text,
        });
    }

    updated.answers.forEach((answer, index) => {
        const originalAnswer = original.answers.find((a) => a.id === answer.id);
        const basePath = `/answers/${index}`;

        if (!originalAnswer) {
            patch.push({
                op: "add",
                path: `/answers/-`,
                value: {
                    questionId: updated.id,
                    text: answer.text,
                    isCorrect: answer.isCorrect,
                    order: index,
                },
            });
        } else {
            if (originalAnswer.text !== answer.text) {
                patch.push({
                    op: "replace",
                    path: `${basePath}/text`,
                    value: answer.text,
                });
            }
            if (originalAnswer.isCorrect !== answer.isCorrect) {
                patch.push({
                    op: "replace",
                    path: `${basePath}/isCorrect`,
                    value: answer.isCorrect,
                });
            }
        }
    });

    // Видалення
    original.answers.forEach((orig) => {
        const stillExists = updated.answers.find((a) => a.id === orig.id);
        if (!stillExists) {
            const index = original.answers.findIndex((a) => a.id === orig.id);
            if (index !== -1) {
                patch.push({
                    op: "remove",
                    path: `/answers/${index}`,
                });
            }
        }
    });

    return patch;
};

export const updateQuestion = (id, updatedQuestion) => async (dispatch, getState) => {
    dispatch({ type: UPDATE_QUESTION_REQUEST });
    console.log(getState().test.questions);
    try {
        const original = getState().test.questions.find((q) => q.id === id);
        if (!original) throw new Error("Оригінальне питання не знайдено");

        const patchDocument = generatePatchDocument(original, updatedQuestion);

        const response = await axios.patch(
            `${baseUrl}/api/Question/update-question/${id}`,
            patchDocument,
            {
                headers: {
                    "Content-Type": "application/json-patch+json",
                },
                withCredentials: true,
            }
        );

        dispatch({
            type: UPDATE_QUESTION_SUCCESS,
            payload: response.data,
            success: response?.data?.success,
        });

        toast.success("Питання оновлено успішно!");
    } catch (e) {
        console.error(e);
        const errors = e.response?.data?.validationErrors;
        dispatch({ type: UPDATE_QUESTION_FAILURE, payload: errors });
        toast.error(e.response?.data?.message || "Помилка при оновленні питання");
    }
};