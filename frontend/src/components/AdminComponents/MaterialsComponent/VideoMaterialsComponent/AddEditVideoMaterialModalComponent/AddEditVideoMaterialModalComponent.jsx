'use client';

import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useForm } from 'react-hook-form';
import { getAllPlanetTopics } from '@/redux/planet/Action';
import { Button } from '@/components/ui/button';
import { ArrowLeft } from 'lucide-react';
import { Input } from '@/components/ui/input';
import {
    Form,
    FormControl,
    FormField,
    FormItem,
    FormLabel,
    FormMessage,
} from '@/components/ui/form';
import { Checkbox } from '@/components/ui/checkbox';
import { createMaterial, getAllMaterials, getAllMaterialThemes, updateMaterial } from '@/redux/material/Action';

const AddEditVideoMaterialModalComponent = ({ video, onBack }) => {
    const dispatch = useDispatch();
    const themes = useSelector(state => state.material.themes);
    console.log(themes);

    const form = useForm({
        defaultValues: {
            id: video?.id || 0,
            link: video?.link || '',
            theme: video?.theme || [],
            type: video?.type || 0,
        },
    });

    const { handleSubmit, setValue, watch } = form;
    const selectedThemes = watch('theme');

    useEffect(() => {
        dispatch(getAllPlanetTopics());
        dispatch(getAllMaterialThemes());
    }, [dispatch]);

    const onCheckboxChange = (id) => {
        const newThemes = selectedThemes.includes(id)
            ? selectedThemes.filter((themeId) => themeId !== id)
            : [...selectedThemes, id];
        setValue('theme', newThemes);
    };

    const onSubmit = async (data) => {
        console.log('Form submitted:', data);
        if (!video.id) {
            const { success } = await dispatch(createMaterial(data));
            if (success) {
                dispatch(getAllMaterials());
                onBack();
            }
        } else {
            const patches = [];

            const keyToPath = {
                content: '/link',
                theme: '/theme',
            };

            for (const key in data) {
                if (data[key] !== video[key] && keyToPath[key]) {
                    patches.push({
                        op: 'replace',
                        path: keyToPath[key],
                        value: data[key],
                    });
                }
            }

            if (patches.length > 0) {
                const { success } = await dispatch(updateMaterial(video.id, patches));
                if (success) {
                    dispatch(getAllMaterials());
                    onBack();
                }
            } else {
                toast.info('Немає змін для збереження.');
            }
        }
    };

    return (
        <div className="relative min-h-[calc(100vh-200px)] flex items-center justify-center px-4">
            <Button
                variant="ghost"
                onClick={onBack}
                className="absolute -top-2 -left-2 z-20 p-2 rounded-full cursor-pointer transition hover:bg-gray-50"
            >
                <ArrowLeft size={28} />
            </Button>

            <div className="flex flex-col items-center justify-center gap-4 w-full max-w-3xl">
                <div className="flex items-center justify-center w-full">
                    <Form {...form}>
                        <form
                            onSubmit={handleSubmit(onSubmit)}
                            className="flex flex-col gap-4 w-full"
                            id="theory-form"
                        >
                            <FormField
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
                                name="link"
                                render={({ field }) => (
                                    <FormItem className="flex flex-col">
                                        <div className="flex items-center gap-4">
                                            <FormLabel>Посилання:</FormLabel>
                                            <FormControl>
                                                <Input placeholder="Посилання на відео ..." {...field} />
                                            </FormControl>
                                        </div>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />

                            <FormField
                                control={form.control}
                                name="theme"
                                render={() => (
                                    <FormItem className="flex flex-col gap-2">
                                        <FormLabel>Теми:</FormLabel>
                                        <div className="flex flex-col gap-1">
                                            {themes &&
                                                Object.entries(themes).map(([id, name]) => {
                                                    const intId = parseInt(id);
                                                    return (
                                                        <label key={id} className="flex items-center gap-2">
                                                            <Checkbox
                                                                checked={selectedThemes.includes(intId)}
                                                                onCheckedChange={() => onCheckboxChange(intId)}
                                                            />
                                                            <span>{name}</span>
                                                        </label>
                                                    );
                                                })}
                                        </div>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />
                        </form>
                    </Form>
                </div>

                <div className="flex items-end justify-end gap-4 w-full mt-4">
                    <Button
                        className="text-sm px-5 py-2 bg-[#2354E1] hover:bg-[#2369e1] text-white cursor-pointer"
                        type="submit"
                        form="theory-form"
                    >
                        {video?.id ? 'Зберегти' : 'Додати'}
                    </Button>
                    <Button className="cursor-pointer" onClick={onBack}>
                        Скасувати
                    </Button>
                </div>
            </div>
        </div>
    );
};

export default AddEditVideoMaterialModalComponent;