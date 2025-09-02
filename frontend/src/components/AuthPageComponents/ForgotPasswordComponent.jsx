import React from 'react';
import {
    Form,
    FormControl,
    FormField,
    FormItem,
    FormLabel,
    FormMessage,
} from '@/components/ui/form';
import { Input } from '@/components/ui/input';
import { Button } from '@/components/ui/button';
import { useForm } from 'react-hook-form';
import { useNavigate } from 'react-router-dom';
import { useDispatch } from 'react-redux';
import { sendResetLink } from '@/redux/auth/Action';

const ForgotPasswordComponent = () => {
    const dispatch = useDispatch();

    const form = useForm({
        defaultValues: {
            email: '',
        },
    });

    const { handleSubmit } = form;

    const onSubmit = (data) => {
        dispatch(sendResetLink(data));
    };

    return (
        <div className="max-w-md mx-auto bg-gray-50 rounded-xl text-center space-y-4">
            <h2 className="text-2xl font-bold uppercase">Забули пароль?</h2>
            <p className="text-gray-700 text-base">
                Ми надішлемо Вам інструкції для його оновлення
            </p>

            <Form {...form}>
                <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
                    <FormField
                        control={form.control}
                        name="email"
                        render={({ field }) => (
                            <FormItem>
                                <FormControl>
                                    <Input
                                        type="email"
                                        placeholder="Ваш email"
                                        {...field}
                                    />
                                </FormControl>
                                <FormMessage />
                            </FormItem>
                        )}
                    />
                    <Button type="submit" className="w-full font-semibold cursor-pointer">
                        Надіслати
                    </Button>
                </form>
            </Form>
        </div>
    );
}

export default ForgotPasswordComponent;