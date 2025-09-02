import React from 'react';
import {
    Form,
    FormControl,
    FormField,
    FormItem,
    FormMessage,
} from '@/components/ui/form';
import { Input } from '@/components/ui/input';
import { Button } from '@/components/ui/button';
import { useForm } from 'react-hook-form';
import { useDispatch } from 'react-redux';
import { getCurrentUser, resetPassword } from '@/redux/auth/Action';
import { useNavigate } from 'react-router-dom';

const ConfirmPasswordComponent = () => {
    const navigate = useNavigate();
    const dispatch = useDispatch();
    const token = new URLSearchParams(window.location.search).get('token');

    const form = useForm({
        defaultValues: {
            password: '',
            confirmPassword: '',
        },
    });

    const { handleSubmit } = form;

    const onSubmit = async (data) => {
        const payload = {
            newPassword: data.password,
            confirmNewPassword: data.confirmPassword,
            token: token  
        };
        
        await dispatch(resetPassword(payload));
        navigate("/");
    };    

    return (
        <div className="bg-gray-50 rounded-xl text-center space-y-4">
            <h2 className="text-2xl font-bold uppercase">ОНОВЛЕННЯ ПАРОЛЮ</h2>

            <Form {...form}>
                <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
                    <FormField
                        control={form.control}
                        name="password"
                        render={({ field }) => (
                            <FormItem>
                                <FormControl>
                                    <Input
                                        type="password"
                                        placeholder="Введіть новий пароль"
                                        {...field}
                                    />
                                </FormControl>
                                <FormMessage />
                            </FormItem>
                        )}
                    />

                    <FormField
                        control={form.control}
                        name="confirmPassword"
                        render={({ field }) => (
                            <FormItem>
                                <FormControl>
                                    <Input
                                        type="password"
                                        placeholder="Повторіть новий пароль"
                                        {...field}
                                    />
                                </FormControl>
                                <FormMessage />
                            </FormItem>
                        )}
                    />

                    <Button type="submit" className="w-full font-semibold cursor-pointer">
                        Оновити пароль
                    </Button>
                </form>
            </Form>
        </div>
    );
};

export default ConfirmPasswordComponent;