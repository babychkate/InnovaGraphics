import React, { useEffect, useState } from 'react';
import { Input } from '@/components/ui/input';
import { Button } from '@/components/ui/button';
import { Form, FormField, FormControl, FormLabel, FormItem, FormMessage } from '@/components/ui/form';
import { useForm } from 'react-hook-form';
import { useDispatch } from 'react-redux';
import { getCurrentUser, verifyCode } from '@/redux/auth/Action';
import { useNavigate } from 'react-router-dom';

const AcceptVerificationCode = () => {
    const navigate = useNavigate();
    const dispatch = useDispatch();

    const form = useForm({
        defaultValues: {
            code: '',
        }
    });

    const { handleSubmit } = form;

    const onSubmit = async (data) => {
        await dispatch(verifyCode(data));
        navigate('/');
    };

    return (
        <div className="w-1/2 bg-gray-50 rounded-xl px-4 py-2">
            <h2 className="text-2xl font-bold text-center mb-4">Введіть верифікаційний код</h2>
            <Form {...form}>
                <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
                    <FormField
                        name="code"
                        control={form.control}
                        render={({ field }) => (
                            <FormItem>
                                <FormControl>
                                    <Input
                                        type="text"
                                        placeholder="Код підтвердження"
                                        {...field}
                                    />
                                </FormControl>
                                <FormMessage />
                            </FormItem>
                        )}
                    />

                    <Button type="submit" className="w-full cursor-pointer font-semibold">
                        Надіслати
                    </Button>
                </form>
            </Form>
        </div>
    );
};

export default AcceptVerificationCode;