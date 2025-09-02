import React, { useEffect, useState } from 'react';
import {
    Form,
    FormControl,
    FormDescription,
    FormField,
    FormItem,
    FormLabel,
    FormMessage
} from '@/components/ui/form';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Checkbox } from '@/components/ui/checkbox';
import { useForm } from 'react-hook-form';
import { useNavigate } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { login, register } from '@/redux/auth/Action';
import { Eye, EyeOff } from 'lucide-react';

const groups = [
    { id: 1, name: "PZ-23" },
    { id: 2, name: "Група Б" },
    { id: 3, name: "Група В" },
];

const RegisterLoginCard = () => {
    const navigate = useNavigate();
    const dispatch = useDispatch();
    const auth = useSelector(state => state.auth);

    const form = useForm({
        defaultValues: {
            realname: '',
            username: '',
            email: '',
            password: '',
            group: 1,
            isteacher: false,
        }
    });

    const { handleSubmit, setError } = form;
    const [isLogin, setIsLogin] = useState(false);
    const [hasNavigated, setHasNavigated] = useState(false);
    const [showPassword, setShowPassword] = useState(false);

    const onSubmit = (data) => {
        if (isLogin) {
            dispatch(login(data));
        } else {
            dispatch(register(data));
        }
    };

    useEffect(() => {
        if (auth.errors && typeof auth.errors === 'object') {
            Object.entries(auth.errors).forEach(([field, messages]) => {
                const normalizedField = field.toLowerCase();

                setError(normalizedField, {
                    type: 'server',
                    message: Array.isArray(messages) ? messages[0] : String(messages),
                });
            });
        }
    }, [auth.errors, setError]);

    useEffect(() => {
        if (!hasNavigated && auth?.errors?.length === 0 && auth?.success) {
            if (auth.lastAction === 'login') {
                navigate('/');
            } else if (auth.lastAction === 'register') {
                navigate('/auth/verification');
            }
            setHasNavigated(true);
        }
    }, [navigate, auth.success, auth.lastAction, hasNavigated]);

    return (
        <div className='w-1/2 bg-gray-50 rounded-xl px-4 py-2'>
            <h2 className='text-2xl font-bold text-center mb-4 uppercase'>
                {!isLogin ? "СТВОРИТИ АКАУНТ" : "УВІЙТИ В АКАУНТ"}
            </h2>

            <Form {...form}>
                <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
                    {!isLogin && (
                        <>
                            <FormField
                                control={form.control}
                                name="realname"
                                render={({ field, fieldState }) => (
                                    <FormItem>
                                        <FormControl>
                                            <Input placeholder="Ваше ім’я та прізвище" {...field} />
                                        </FormControl>
                                        {!fieldState.error && (
                                            <FormDescription>
                                                Вкажіть ваше справжнє ім’я та прізвище українською для відображення на сертифікаті.
                                            </FormDescription>
                                        )}
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />

                            <FormField
                                control={form.control}
                                name="username"
                                render={({ field, fieldState }) => (
                                    <FormItem>
                                        <FormControl>
                                            <Input placeholder="Ваш нікнейм для програми" {...field} />
                                        </FormControl>
                                        {!fieldState.error && (
                                            <FormDescription>
                                                Нікнейм повинен містити латинські літери і цифри, без підкреслень і спец. символів.
                                            </FormDescription>
                                        )}
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />
                        </>
                    )}

                    <FormField
                        control={form.control}
                        name="email"
                        render={({ field, fieldState }) => (
                            <FormItem>
                                <FormControl>
                                    <Input placeholder="Ваш email" {...field} />
                                </FormControl>
                                {!fieldState.error && (
                                    <FormDescription>
                                        Вкажіть дійсну @lpnu.ua електронну адресу для входу та підтвердження акаунту.
                                    </FormDescription>
                                )}
                                <FormMessage />
                            </FormItem>
                        )}
                    />

                    <FormField
                        control={form.control}
                        name="password"
                        render={({ field, fieldState }) => (
                            <FormItem>
                                <FormControl>
                                    <div className="relative">
                                        <Input
                                            placeholder="Пароль"
                                            type={showPassword ? "text" : "password"}
                                            {...field}
                                            className="pr-10"
                                        />
                                        <button
                                            type="button"
                                            onClick={() => setShowPassword(prev => !prev)}
                                            className="absolute right-3 top-1/2 transform -translate-y-1/2 text-gray-500 hover:text-black"
                                            tabIndex={-1}
                                        >
                                            {showPassword ? <EyeOff size={20} /> : <Eye size={20} />}
                                        </button>
                                    </div>
                                </FormControl>
                                {!fieldState.error && (
                                    <FormDescription>
                                        Пароль повинен містити щонайменше 6 символів, містити велику літеру, маленьку літеру, цифру і спец. символ.
                                    </FormDescription>
                                )}
                                <FormMessage />
                                {isLogin && (
                                    <div className="text-right mt-1">
                                        <Button
                                            type="button"
                                            variant="link"
                                            className="text-sm text-black hover:underline p-0"
                                            onClick={() => navigate('/auth/forgot-password')}
                                        >
                                            Забули пароль?
                                        </Button>
                                    </div>
                                )}
                            </FormItem>
                        )}
                    />

                    {!isLogin && (
                        <>
                            <FormField
                                control={form.control}
                                name="group"
                                render={({ field, fieldState }) => (
                                    <FormItem>
                                        <FormControl>
                                            <select
                                                {...field}
                                                className="w-full border border-gray-300 rounded-md p-2"
                                            >
                                                {groups.map((group) => (
                                                    <option key={group.id} value={group.id}>
                                                        {group.name}
                                                    </option>
                                                ))}
                                            </select>
                                        </FormControl>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />

                            <FormField
                                control={form.control}
                                name="isteacher"
                                render={({ field }) => (
                                    <FormItem className="flex items-center space-x-2">
                                        <FormControl>
                                            <Checkbox
                                                checked={field.value}
                                                onCheckedChange={field.onChange}
                                            />
                                        </FormControl>
                                        <FormLabel className="m-0">
                                            Я викладач
                                        </FormLabel>
                                    </FormItem>
                                )}
                            />
                        </>
                    )}

                    <Button type="submit" className="w-full cursor-pointer font-semibold">
                        {isLogin ? 'Увійти в аккаунт' : 'Зареєструватися'}
                    </Button>
                </form>
            </Form>

            <div className="text-center mt-4 text-base">
                <span className="text-base">{isLogin ? "Ще не маєте акаунту?" : 'Уже маєте акаунт?'}</span>
                <Button
                    variant="link"
                    className="cursor-pointer text-base"
                    onClick={() => setIsLogin((prev) => !prev)}
                >
                    {isLogin ? 'Зареєструватися' : 'Увійти'}
                </Button>
            </div>
        </div>
    );
};

export default RegisterLoginCard;