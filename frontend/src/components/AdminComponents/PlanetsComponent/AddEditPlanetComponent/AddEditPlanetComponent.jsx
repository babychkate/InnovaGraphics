'use client';

import Planet from '@/components/AuthPageComponents/Planet';
import { Button } from '@/components/ui/button';
import {
    Form,
    FormControl,
    FormField,
    FormItem,
    FormLabel,
    FormMessage
} from '@/components/ui/form';
import { Input } from '@/components/ui/input';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { ArrowLeft, Pencil } from 'lucide-react';
import { Canvas } from '@react-three/fiber';
import { OrbitControls } from '@react-three/drei';
import { useForm } from 'react-hook-form';
import React, { useEffect, useRef } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { createPlanet, getAllPlanets, getAllPlanetSubTopics, getAllPlanetTopics, updatePlanet } from '@/redux/planet/Action';
import { toast } from 'react-toastify';

const AddEditPlanetComponent = ({ planet = {}, onBack }) => {
    const dispatch = useDispatch();
    const planetState = useSelector((state) => state.planet);
    const topics = useSelector((state) => state.planet.topics);
    const subTopics = useSelector((state) => state.planet.subTopics);

    const form = useForm({
        defaultValues: {
            id: planet.id || 0,
            name: planet.name || '',
            topic: planet.topic || '',
            subTopic: planet.subTopic || '',
            requiredEnergy: planet.requiredEnergy || 20,
            maxHintCount: planet.maxHintCount || 1,
            number: planet.number || 1,
            energyLost: planet.energyLost || 10,
            imagePath: planet.imagePath || "/planet_texture4.jpg",
        },
    });

    const { handleSubmit } = form;
    const values = form.watch();

    const serverFieldMap = {
        subtopic: 'subTopic',
        topic: 'topic',
        name: 'name',
        requiredenergy: 'requiredEnergy',
        maxhintcount: 'maxHintCount',
        number: 'number',
        energylost: 'energyLost',
        imagepath: 'imagePath',
    };

    useEffect(() => {
        dispatch(getAllPlanetTopics());
        dispatch(getAllPlanetSubTopics());
    }, [dispatch]);

    useEffect(() => {
        if (planetState.errors && typeof planetState.errors === 'object') {
            Object.entries(planetState.errors).forEach(([field, messages]) => {
                const normalizedField = field.toLowerCase();
                const formField = serverFieldMap[normalizedField] || normalizedField;

                form.setError(formField, {
                    type: 'server',
                    message: Array.isArray(messages) ? messages[0] : String(messages),
                });
            });
        }
    }, [planetState.errors]);

    const fileInputRef = useRef(null);

    const handleImageUpload = (e) => {
        const file = e.target.files?.[0];
        if (file) {
            const imageUrl = URL.createObjectURL(file);
            form.setValue('imagePath', imageUrl);
        }
    };

    const handleUploadClick = () => {
        fileInputRef.current?.click();
    };

    const onSubmit = async (data) => {
        if (!data.id) {
            const { success } = await dispatch(createPlanet(data));
            if (success) {
                dispatch(getAllPlanets());
                onBack();
            }
        } else {
            const patches = [];

            for (const key in data) {
                if (data[key] !== planet[key]) {
                    patches.push({
                        op: 'replace',
                        path: `/${key}`,
                        value: data[key],
                    });
                }
            }

            if (patches.length > 0) {
                const { success } = await dispatch(updatePlanet(planet.id, patches));
                if (success) {
                    dispatch(getAllPlanets());
                    onBack();
                }
            } else {
                toast.info('Немає змін для збереження.');
            }
        }
    };

    return (
        <div className="relative min-h-[calc(100vh-100px)] flex flex-col items-center justify-center bg-white">
            <Button
                variant="ghost"
                onClick={onBack}
                className="absolute top-1 left-1 z-20 p-2 rounded-full cursor-pointer transition hover:bg-gray-50"
            >
                <ArrowLeft size={28} />
            </Button>

            <div className="flex items-center justify-center gap-30 w-full">
                <Form {...form}>
                    <form onSubmit={handleSubmit(onSubmit)} id="planet-form" className="flex flex-col gap-4 w-96">
                        <FormField
                            name="id"
                            control={form.control}
                            render={({ field }) => (
                                <FormItem className="hidden">
                                    <FormControl>
                                        <Input type="hidden" {...field} />
                                    </FormControl>
                                </FormItem>
                            )}
                        />

                        <FormField
                            name="name"
                            control={form.control}
                            render={({ field }) => (
                                <FormItem className="flex flex-col">
                                    <div className="flex items-center gap-4">
                                        <FormLabel>Назва</FormLabel>
                                        <FormControl>
                                            <Input placeholder="Назва планети" {...field} />
                                        </FormControl>
                                    </div>
                                    <FormMessage />
                                </FormItem>
                            )}
                        />

                        <FormField
                            name="topic"
                            control={form.control}
                            render={({ field }) => (
                                <FormItem className="flex flex-col">
                                    <div className="flex items-center gap-4">
                                        <FormLabel>Тема</FormLabel>
                                        <FormControl>
                                            <Input
                                                placeholder="Введіть тему"
                                                {...field}
                                            />
                                        </FormControl>
                                    </div>
                                    <FormMessage />
                                </FormItem>
                            )}
                        />

                        <FormField
                            name="subTopic"
                            control={form.control}
                            render={({ field }) => (
                                <FormItem className="flex flex-col">
                                    <div className="flex items-center gap-4">
                                        <FormLabel>Підтема</FormLabel>
                                        <FormControl>
                                            <Input
                                                placeholder="Введіть підтему"
                                                {...field}
                                            />
                                        </FormControl>
                                    </div>
                                    <FormMessage />
                                </FormItem>
                            )}
                        />

                        {[
                            { name: "requiredEnergy", label: "Мінімальна енергія", type: "number", placeholder: "Введіть мінімальну енергію" },
                            { name: "maxHintCount", label: "Максимум підказок", type: "number", placeholder: "Введіть кількість підказок" },
                            { name: "number", label: "Номер", type: "number", placeholder: "Введіть номер планети" },
                            { name: "energyLost", label: "Втрата енергії", type: "number", placeholder: "Введіть втрату енергії" },
                        ].map(({ name, label, type, placeholder }) => (
                            <FormField
                                key={name}
                                control={form.control}
                                name={name}
                                render={({ field }) => (
                                    <FormItem className="flex flex-col">
                                        <div className="flex items-center gap-4">
                                            <FormLabel className="whitespace-nowrap text-right">{label}</FormLabel>
                                            <FormControl>
                                                <Input placeholder={placeholder} type={type} {...field} />
                                            </FormControl>
                                        </div>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />
                        ))}
                    </form>
                </Form>

                <div className="relative h-100 w-100">
                    <Canvas camera={{ position: [0, 0, 1.75] }}>
                        <ambientLight />
                        <directionalLight position={[5, 5, 5]} />
                        <Planet size={0.9} textureUrl="/planet_texture4.jpg" clickable={false} />
                        <OrbitControls enableZoom={false} />
                    </Canvas>

                    <Button
                        type="button"
                        variant="ghost"
                        size="icon"
                        className="absolute bottom-10 right-10 rounded-full cursor-pointer bg-white shadow hover:bg-muted"
                        onClick={handleUploadClick}
                    >
                        <Pencil size={20} />
                    </Button>

                    <Input
                        type="file"
                        accept="image/*"
                        ref={fileInputRef}
                        onChange={handleImageUpload}
                        className="hidden"
                    />
                </div>
            </div>

            <div className="flex items-end justify-end gap-4 w-full mt-4">
                <Button
                    className="text-sm px-5 py-2 bg-[#2354E1] hover:bg-[#2369e1] cursor-pointer text-white hover:text-white"
                    variant="ghost"
                    type="submit"
                    form="planet-form"
                >
                    {planet.id ? 'Зберегти' : 'Додати'}
                </Button>
                <Button className="cursor-pointer" onClick={onBack}>
                    Скасувати
                </Button>
            </div>
        </div>
    );
};

export default AddEditPlanetComponent;