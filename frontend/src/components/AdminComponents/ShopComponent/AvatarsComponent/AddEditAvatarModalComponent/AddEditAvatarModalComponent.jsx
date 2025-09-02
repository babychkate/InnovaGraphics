import React, { useRef } from 'react';

import { Button } from '@/components/ui/button';
import { ArrowLeft, Pencil } from 'lucide-react';
import { Input } from '@/components/ui/input';
import {
    Form,
    FormControl,
    FormField,
    FormItem,
    FormLabel,
    FormMessage,
} from '@/components/ui/form';
import { createAvatar, getAllAvatars, updateAvatar } from '@/redux/shop_items/Action';
import { toast } from 'react-toastify';
import { useForm } from 'react-hook-form';
import { useDispatch } from 'react-redux';

const AddEditAvatarModalComponent = ({ avatar, onBack }) => {
    const dispatch = useDispatch();

    const form = useForm({
        defaultValues: {
            id: avatar?.id || 0,
            name: avatar?.name || '',
            photoPath: avatar?.photoPath || '',
            type: avatar?.type || 0,
            price: avatar?.price || 0,
        },
    });

    const { handleSubmit } = form;

    const fileInputRef = useRef(null);

    const handleImageUpload = (e) => {
        const file = e.target.files?.[0];
        if (file) {
            const imageUrl = URL.createObjectURL(file);
            form.setValue('photoPath', imageUrl);
            form.setValue('file', file);
        }
    };

    const handleUploadClick = () => {
        fileInputRef.current?.click();
    };

    const onSubmit = async (data) => {
        console.log('Form submitted:', data);

        if (!avatar.id) {
            const formData = new FormData();
            formData.append('name', data.name);
            formData.append('price', data.price);
            formData.append('type', data.type);
            if (data.file) {
                formData.append('photoPath', data.photoPath);
            }

            const { success } = await dispatch(createAvatar(formData));
            if (success) {
                dispatch(getAllAvatars());
                onBack();
            }
        } else {
            const patches = [];

            const keyToPath = {
                content: '/name',
                price: '/price',
                photoPath: '/photoPath',
            };

            for (const key in data) {
                if (data[key] !== avatar[key] && keyToPath[key]) {
                    patches.push({
                        op: 'replace',
                        path: keyToPath[key],
                        value: data[key],
                    });
                }
            }

            if (patches.length > 0) {
                const { success } = await dispatch(updateAvatar(avatar.id, patches));
                if (success) {
                    dispatch(getAllAvatars());
                    onBack();
                }
            } else {
                toast.info('Немає змін для збереження.');
            }
        }
    }

    return (
        <div className="relative min-h-[calc(100vh-200px)] flex flex-col items-center justify-center bg-white">
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
                                            <Input placeholder="Назва аватарки" {...field} />
                                        </FormControl>
                                    </div>
                                    <FormMessage />
                                </FormItem>
                            )}
                        />

                        <FormField
                            name="price"
                            control={form.control}
                            render={({ field }) => (
                                <FormItem className="flex flex-col">
                                    <div className="flex items-center gap-4">
                                        <FormLabel>Ціна</FormLabel>
                                        <FormControl>
                                            <Input
                                                placeholder="Ціна аватарки"
                                                {...field}
                                            />
                                        </FormControl>
                                    </div>
                                    <FormMessage />
                                </FormItem>
                            )}
                        />
                    </form>
                </Form>

                <div className="relative flex items-center justify-center">
                    <img
                        className='h-70 w-70 rounded-full'
                        src={form.watch("photoPath") || "/avatar2.png"}
                        alt="Avatar preview"
                    />

                    <Button
                        type="button"
                        variant="ghost"
                        size="icon"
                        className="absolute bottom-0 right-0 rounded-full cursor-pointer bg-white shadow hover:bg-muted"
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
                    {avatar.id ? 'Зберегти' : 'Додати'}
                </Button>
                <Button className="cursor-pointer" onClick={onBack}>
                    Скасувати
                </Button>
            </div>
        </div>
    );
}

export default AddEditAvatarModalComponent;