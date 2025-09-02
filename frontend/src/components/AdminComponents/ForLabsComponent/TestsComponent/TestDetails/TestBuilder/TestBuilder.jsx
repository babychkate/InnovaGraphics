import { useEffect, useRef, useState } from 'react';

import { CSS } from '@dnd-kit/utilities';
import { Input } from '@/components/ui/input';
import { Button } from '@/components/ui/button';
import { Textarea } from '@/components/ui/textarea';
import { Card, CardContent } from '@/components/ui/card';
import { PlusCircle, Trash2, GripVertical } from 'lucide-react';
import { closestCenter, DndContext, PointerSensor, useSensor, useSensors } from '@dnd-kit/core';
import { arrayMove, SortableContext, useSortable, verticalListSortingStrategy } from '@dnd-kit/sortable';
import { useDispatch, useSelector } from 'react-redux';
import { addQuestionAnswers, deleteQuestionFromTest, getTestWithQuestions } from '@/redux/test/Action';
import { updateQuestion } from '@/redux/question/Action';
import { createAnswer } from '@/redux/answer/Action';


function SortableItem({ id, children }) {
    const { attributes, listeners, setNodeRef, transform, transition } = useSortable({ id });
    const style = { transform: CSS.Transform.toString(transform), transition };

    return (
        <div ref={setNodeRef} style={style}>
            <div className="flex items-start gap-2">
                <div {...attributes} {...listeners} className="cursor-grab mt-1 p-1">
                    <GripVertical className="text-gray-400" />
                </div>
                <div className="flex-grow">{children}</div>
            </div>
        </div>
    );
}

const TestBuilder = ({ testId }) => {
    const dispatch = useDispatch();
    const sensors = useSensors(useSensor(PointerSensor));
    const existingQuestionIds = useRef(new Set());
    const originalQuestionsRef = useRef([]);

    const [testName, setTestName] = useState('');
    const [questions, setQuestions] = useState([]);

    const fetchedQuestions = useSelector((state) => state.test.questions);

    useEffect(() => {
        if (testId) {
            dispatch(getTestWithQuestions(testId));
        }
    }, [testId, dispatch]);

    useEffect(() => {
        if (fetchedQuestions?.length) {
            const mapped = fetchedQuestions.map((q) => ({
                id: q?.id,
                text: q?.text,
                answers: q?.answers?.map((a) => ({
                    id: a?.id,
                    text: a.text,
                    isCorrect: a.isCorrect,
                })) || [],
            }));
            setQuestions(mapped);
            originalQuestionsRef.current = JSON.parse(JSON.stringify(mapped));
            mapped.forEach(q => existingQuestionIds.current.add(q.id));
        } else {
            setQuestions([]);
        }
    }, [fetchedQuestions]);

    const isQuestionChanged = (qIdx) => {
        const original = originalQuestionsRef.current[qIdx];
        const current = questions[qIdx];
        if (!original || !current) return false;
        if (original.text !== current.text) return true;
        if (original.answers.length !== current.answers.length) return true;
        for (let i = 0; i < original.answers.length; i++) {
            if (
                original.answers[i].text !== current.answers[i].text ||
                original.answers[i].isCorrect !== current.answers[i].isCorrect
            ) {
                return true;
            }
        }
        return false;
    };

    const handleQuestionDragEnd = (event) => {
        const { active, over } = event;
        if (active.id !== over?.id) {
            const oldIndex = questions.findIndex((q) => q.id === active.id);
            const newIndex = questions.findIndex((q) => q.id === over.id);
            setQuestions((items) => arrayMove(items, oldIndex, newIndex));
        }
    };

    const handleOptionDragEnd = (qIndex) => (event) => {
        const { active, over } = event;
        if (active.id !== over?.id) {
            const answers = questions[qIndex].answers;
            const oldIndex = answers.findIndex((o) => o.id === active.id);
            const newIndex = answers.findIndex((o) => o.id === over.id);
            const updatedQuestions = [...questions];
            updatedQuestions[qIndex].answers = arrayMove(answers, oldIndex, newIndex);
            setQuestions(updatedQuestions);
        }
    };

    const handleRemoveQuestion = (index, id) => {
        dispatch(deleteQuestionFromTest(id));
        setQuestions(questions.filter((_, i) => i !== index));
    };

    const handleQuestionChange = (index, value) => {
        const updated = [...questions];
        updated[index].text = value;
        setQuestions(updated);
    };

    const handleOptionTextChange = (qIndex, oIndex, value) => {
        const updated = [...questions];
        updated[qIndex].answers[oIndex].text = value;
        setQuestions(updated);
    };

    const handleOptionToggleCorrect = (qIndex, oIndex) => {
        const updated = [...questions];
        updated[qIndex].answers[oIndex].isCorrect = !updated[qIndex].answers[oIndex].isCorrect;
        setQuestions(updated);
    };

    const handleAddOption = (qIndex) => {
        const updated = [...questions];
        updated[qIndex].answers.push({ id: crypto.randomUUID(), text: '', isCorrect: false, questionId: updated[qIndex].id });
        setQuestions(updated);
    };

    const handleRemoveOption = (qIndex, oIndex) => {
        const updated = [...questions];
        updated[qIndex].answers.splice(oIndex, 1);
        setQuestions(updated);
    };

    const handleAddQuestion = () => {
        const newId = crypto.randomUUID();
        setQuestions([
            ...questions,
            {
                id: newId,
                text: '',
                answers: [
                    { id: crypto.randomUUID(), text: '', isCorrect: false },
                    { id: crypto.randomUUID(), text: '', isCorrect: false },
                ],
            },
        ]);
    };

    const handleSave = () => {
        const newQuestions = questions.filter(q => !existingQuestionIds.current.has(q.id));
        const startNumber = existingQuestionIds.current.size;
        const formattedData = newQuestions.map((q, index) => ({
            number: startNumber + index + 1,
            text: q.text,
            answers: q.answers.map((o) => ({
                text: o.text,
                isCorrect: o.isCorrect,
            })),
        }));

        if (formattedData.length > 0) {
            dispatch(addQuestionAnswers(testId, formattedData));
            newQuestions.forEach(q => existingQuestionIds.current.add(q.id));
        }
    };

    return (
        <div className="p-6">
            <h1 className="text-2xl font-bold mb-4">Конструктор тестів</h1>

            <Input
                placeholder="Назва тесту"
                value={testName}
                onChange={(e) => setTestName(e.target.value)}
                className="mb-6"
            />

            <DndContext sensors={sensors} collisionDetection={closestCenter} onDragEnd={handleQuestionDragEnd}>
                <SortableContext items={questions.map((q) => q.id)} strategy={verticalListSortingStrategy}>
                    {questions.map((q, qIdx) => (
                        <SortableItem key={q.id} id={q.id}>
                            <Card className="mb-6">
                                <CardContent className="space-y-4">
                                    <div className="flex justify-between items-start gap-4">
                                        <Textarea
                                            value={q.text}
                                            placeholder={`Питання ${qIdx + 1}`}
                                            onChange={(e) => handleQuestionChange(qIdx, e.target.value)}
                                        />
                                        <div className="flex flex-col gap-2 items-end">
                                            {isQuestionChanged(qIdx) && existingQuestionIds.current.has(q.id) && (
                                                <Button
                                                    className="bg-yellow-200 hover:bg-yellow-300 text-yellow-800 px-3 py-1 text-sm"
                                                    onClick={() => {
                                                        const updated = questions[qIdx];
                                                        console.log(updated);
                                                        dispatch(updateQuestion(updated.id, updated));
                                                        originalQuestionsRef.current[qIdx] = JSON.parse(JSON.stringify(updated));
                                                    }}
                                                >
                                                    Зберегти питання
                                                </Button>
                                            )}
                                            <Button
                                                variant="ghost"
                                                size="icon"
                                                onClick={(e) => {
                                                    e.stopPropagation();
                                                    handleRemoveQuestion(qIdx, q.id);
                                                }}
                                                className="text-red-500"
                                            >
                                                <Trash2 />
                                            </Button>
                                        </div>
                                    </div>

                                    <DndContext
                                        sensors={sensors}
                                        collisionDetection={closestCenter}
                                        onDragEnd={handleOptionDragEnd(qIdx)}
                                    >
                                        <SortableContext
                                            items={q.answers.map((opt) => opt.id)}
                                            strategy={verticalListSortingStrategy}
                                        >
                                            {q.answers.map((opt, optIdx) => (
                                                <SortableItem key={opt.id} id={opt.id}>
                                                    <div className="flex items-center gap-2">
                                                        <input
                                                            type="checkbox"
                                                            checked={opt.isCorrect}
                                                            onChange={() => handleOptionToggleCorrect(qIdx, optIdx)}
                                                            title="Правильна відповідь"
                                                        />
                                                        <Input
                                                            value={opt.text}
                                                            onChange={(e) =>
                                                                handleOptionTextChange(qIdx, optIdx, e.target.value)
                                                            }
                                                            placeholder={`Варіант ${optIdx + 1}`}
                                                            className="flex-grow"
                                                        />
                                                        {q.answers.length > 2 && (
                                                            <Button
                                                                size="icon"
                                                                variant="ghost"
                                                                className="text-red-500"
                                                                onClick={(e) => {
                                                                    e.stopPropagation();
                                                                    handleRemoveOption(qIdx, optIdx);
                                                                }}
                                                                title="Видалити варіант"
                                                            >
                                                                <Trash2 size={16} />
                                                            </Button>
                                                        )}
                                                    </div>
                                                </SortableItem>
                                            ))}
                                        </SortableContext>
                                    </DndContext>

                                    <Button
                                        variant="outline"
                                        size="sm"
                                        onClick={() => handleAddOption(qIdx)}
                                        className="mt-2"
                                    >
                                        Додати варіант
                                    </Button>
                                </CardContent>
                            </Card>
                        </SortableItem>
                    ))}
                </SortableContext>
            </DndContext>

            <Button
                onClick={handleAddQuestion}
                className="w-full mt-4 mb-6 cursor-pointer bg-blue-100 hover:bg-blue-200 text-blue-800 font-medium py-3 rounded-lg flex items-center justify-center"
            >
                <PlusCircle className="mr-2" /> Додати питання
            </Button>

            <div className="flex justify-end">
                <Button
                    onClick={handleSave}
                    className="text-sm px-5 py-2 bg-[#2354E1] hover:bg-[#2369e1] cursor-pointer text-white hover:text-white"
                >
                    Зберегти тест
                </Button>
            </div>
        </div>
    );
};

export default TestBuilder;