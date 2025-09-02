import { useEffect } from 'react';
import { Button } from '@/components/ui/button';
import { ArrowLeft } from 'lucide-react';
import { useForm } from 'react-hook-form';
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from '@/components/ui/form';
import { Input } from '@/components/ui/input';
import { createTest, getAllTests, updateTest } from '@/redux/test/Action';
import { useDispatch, useSelector } from 'react-redux';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { getAllPlanets, getAllPlanetTopics } from '@/redux/planet/Action';
import { toast } from 'react-toastify';

const AddEditTestComponent = ({ test = {}, onBack }) => {
    const dispatch = useDispatch();
    const planets = useSelector(state => state?.planet?.planets);
    const themes = useSelector((state) => state.planet.topics);
    const testState = useSelector(state => state?.test);
    console.log(themes);

    const form = useForm({
        defaultValues: {
            id: test.id || 0,
            name: test.name || '',
            theme: test.theme || '',
            planetName: test.planetName || '',
            timeLimit: test.timeLimit || '00:10:00',
        },
    });

    const { handleSubmit } = form;

    const serverFieldMap = {
        testname: 'name',
        planetName: 'planetName',
        theme: 'theme',
    };

    useEffect(() => {
        if (testState.errors && typeof testState.errors === 'object') {
            Object.entries(testState.errors).forEach(([field, messages]) => {
                const formField = serverFieldMap[field];
                console.log(formField)
                form.setError(formField, {
                    type: 'server',
                    message: Array.isArray(messages) ? messages[0] : String(messages),
                });
            });
        }
    }, [testState.errors]);


    useEffect(() => {
        dispatch(getAllPlanets());
        dispatch(getAllPlanetTopics());
    }, [dispatch]);

    const onSubmit = async (data) => {
        if (!data.id) {
            const { success } = await dispatch(createTest(data));
            if (success) {
                dispatch(getAllTests());
                onBack();
            }
        } else {
            const patches = [];

            const keyToPath = {
                name: '/name',
                theme: '/theme',
                timeLimit: '/timelimit',
            };

            for (const key in data) {
                if (data[key] !== test[key] && keyToPath[key]) {
                    patches.push({
                        op: 'replace',
                        path: keyToPath[key],
                        value: data[key],
                    });
                }
            }

            if (patches.length > 0) {
                const { success } = await dispatch(updateTest(test.id, patches));
                if (success) {
                    dispatch(getAllTests());
                    onBack();
                }
            } else {
                toast.info('Немає змін для збереження.');
            }
        }
    };

    return (
        <div className={`relative min-h-[calc(100vh-200px)] flex items-center justify-center`}>
            <Button
                variant="ghost"
                onClick={onBack}
                className="absolute -top-2 -left-2 z-20 p-2 rounded-full cursor-pointer transition hover:bg-gray-50"
            >
                <ArrowLeft size={28} />
            </Button>

            <div className="flex flex-col items-center justify-center gap-4">
                <div className="flex items-center justify-center gap-30 w-full">
                    <Form {...form}>
                        <form onSubmit={handleSubmit(onSubmit)} className="flex flex-col gap-4 w-96" id="test-form">
                            <FormField
                                key="id"
                                control={form.control}
                                name="id"
                                render={({ field }) => (
                                    <FormItem className="hidden">
                                        <FormControl>
                                            <Input type="hidden" {...field} />
                                        </FormControl>
                                    </FormItem>
                                )}
                            />
                            <FormField
                                control={form.control}
                                name="name"
                                render={({ field }) => (
                                    <FormItem className="flex flex-col">
                                        <div className="flex items-center gap-4">
                                            <FormLabel>Назва:</FormLabel>
                                            <FormControl>
                                                <Input placeholder="Назва тесту" {...field} />
                                            </FormControl>
                                        </div>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />
                            <FormField
                                name="theme"
                                control={form.control}
                                render={({ field }) => (
                                    <FormItem className="flex flex-col">
                                        <div className="flex items-center gap-4">
                                            <FormLabel>Тема</FormLabel>
                                            <Select value={field.value} onValueChange={field.onChange}>
                                                <FormControl>
                                                    <SelectTrigger className="w-full">
                                                        <SelectValue placeholder="Оберіть тему" />
                                                    </SelectTrigger>
                                                </FormControl>
                                                <SelectContent>
                                                    {themes?.map((theme) => (
                                                        <SelectItem key={theme} value={theme}>
                                                            {theme}
                                                        </SelectItem>
                                                    ))}
                                                </SelectContent>
                                            </Select>
                                        </div>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />
                            <FormField
                                control={form.control}
                                name="planetName"
                                render={({ field }) => (
                                    <FormItem className="flex flex-col">
                                        <div className="flex items-center gap-4">
                                            <FormLabel>Планета:</FormLabel>
                                            <FormControl>
                                                <Select
                                                    onValueChange={field.onChange}
                                                    defaultValue={field.value}
                                                >
                                                    <SelectTrigger>
                                                        <SelectValue placeholder="Оберіть планету" />
                                                    </SelectTrigger>
                                                    <SelectContent>
                                                        {planets.map((planet) => (
                                                            <SelectItem key={planet.id} value={planet.name}>
                                                                {planet.name}
                                                            </SelectItem>
                                                        ))}
                                                    </SelectContent>
                                                </Select>
                                            </FormControl>
                                        </div>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />
                            <FormField
                                control={form.control}
                                name="timeLimit"
                                render={({ field }) => (
                                    <FormItem className="flex flex-col">
                                        <div className="flex items-center gap-4">
                                            <FormLabel>Часовий ліміт:</FormLabel>
                                            <FormControl>
                                                <input
                                                    {...field}
                                                    placeholder="Час"
                                                    type="time"
                                                    step="1"
                                                    value={test?.timeLimit}
                                                    className="border-none outline-none"
                                                />
                                            </FormControl>
                                        </div>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />
                        </form>
                    </Form>
                </div>

                <div className='flex items-end justify-end gap-4 w-full mt-4'>
                    <Button
                        className="text-sm px-5 py-2 bg-[#2354E1] hover:bg-[#2369e1] cursor-pointer text-white hover:text-white"
                        variant="ghost"
                        type="submit"
                        form="test-form"
                    >
                        {test.id ? 'Зберегти' : 'Додати'}
                    </Button>
                    <Button className="cursor-pointer">
                        Скасувати
                    </Button>
                </div>
            </div>
        </div>
    );
};

export default AddEditTestComponent;