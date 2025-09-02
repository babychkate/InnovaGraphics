import React, { useEffect, useState } from 'react';
import { Input } from '@/components/ui/input';
import { Button } from '@/components/ui/button';
import { Textarea } from '@/components/ui/textarea';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { FaInstagram, FaGithub, FaLinkedin } from 'react-icons/fa';
import { Canvas } from '@react-three/fiber';
import Planet from '@/components/AuthPageComponents/Planet';
import { OrbitControls } from '@react-three/drei';
import MoveBack from '@/components/MoveBack/MoveBack';
import { useDispatch, useSelector } from 'react-redux';
import { updateUserProfile } from '@/redux/user/Action';
import { toast } from 'react-toastify';
import { getImageById } from '@/redux/image/Action';

const avatarOptions = [
    { id: '11926829-E348-48F5-A635-97920F644110', url: '/team/roksolana.jpg' },
    { id: '44882095-F0C6-4009-8523-453E99FA1BB6', url: '/team/katia.jpg' },
];

const UpdateUserProfilePage = () => {
    const dispatch = useDispatch();
    const user = useSelector(state => state.auth.user);
    const avatar = useSelector(state => state.image.imageBase64);

console.log("Avatar:", avatar);

    const [formData, setFormData] = useState({
        realName: user?.realName || '',
        email: user?.email || '',
        userName: user?.userName || '',
        password: '',
        group: user?.group || '',
        avatarId: user?.profile?.avatarId,
        aboutMyself: user?.profile?.aboutMyself || '',
        instagramLink: user?.profile?.instagramLink || '',
        gitHubLink: user?.profile?.gitHubLink || '',
        linkedInLink: user?.profile?.linkedInLink || '',
    });

    const handleChange = (field, value) => {
        setFormData(prev => ({ ...prev, [field]: value }));
    };

    // useEffect(() => {
    //     dispatch(getImageById("11926829-E348-48F5-A635-97920F644110")); // HERE NEED TO UPDATE
    // }, [dispatch]);

    const handleSave = () => {
        const patches = [];

        if (formData.userName !== user?.userName) {
            patches.push({
                op: "replace",
                path: "/userName",
                value: formData.userName,
            });
        }

        if (formData.realName !== user?.realName) {
            patches.push({
                op: "replace",
                path: "/realName",
                value: formData.realName,
            });
        }

        if (formData.avatarId !== user?.profile?.avatarId) {
            patches.push({
                op: "replace",
                path: "/profile/avatarId",
                value: formData.avatarId,
            });
        }

        if (formData.aboutMyself !== user?.profile?.aboutMyself) {
            patches.push({
                op: "replace",
                path: "/profile/AboutMyself",
                value: formData.aboutMyself,
            });
        }

        if (formData.instagramLink !== user?.profile?.instagramLink) {
            patches.push({
                op: "replace",
                path: "/profile/InstagramLink",
                value: formData.instagramLink,
            });
        }

        if (formData.gitHubLink !== user?.profile?.gitHubLink) {
            patches.push({
                op: "replace",
                path: "/profile/GitHubLink",
                value: formData.gitHubLink,
            });
        }

        if (formData.linkedInLink !== user?.profile?.linkedInLink) {
            patches.push({
                op: "replace",
                path: "/profile/LinkedInLink",
                value: formData.linkedInLink,
            });
        }

        if (patches.length === 0) {
            toast.info("Змін немає для збереження.");
            return;
        }

        console.log(formData, user?.profile);
        dispatch(updateUserProfile(user?.id, patches));
    };

    return (
        <div className='pt-[72px]'>
            <MoveBack to="/my-profile" />

            <div className='relative z-10 flex items-center justify-center gap-10 h-[calc(100vh-100px)] px-4'>
                <div className='flex flex-col gap-4'>
                    <div className='relative mt-[-99px] flex flex-col gap-4'>
                        <img src={avatar || "https://cdn-icons-png.flaticon.com/512/149/149071.png"} alt="Avatar" className='w-45 h-45 rounded-full' />

                        <Select
                            value={formData.avatarId}
                            onValueChange={(value) => handleChange('avatarId', value)}
                        >
                            <SelectTrigger className="w-full">
                                <SelectValue placeholder="Оберіть аватар" />
                            </SelectTrigger>
                            <SelectContent>
                                {avatarOptions.map((avatar) => (
                                    <SelectItem key={avatar.id} value={avatar.id}>
                                        <img src={avatar || "https://cdn-icons-png.flaticon.com/512/149/149071.png"} alt="avatar" className="w-8 h-8 rounded-full" />
                                    </SelectItem>
                                ))}
                            </SelectContent>
                        </Select>
                    </div>

                    <div className='flex flex-col gap-3'>
                        <div className='flex items-center gap-2'>
                            <FaInstagram size={20} />
                            <Input placeholder='Instagram' value={formData.instagramLink} onChange={(e) => handleChange('instagramLink', e.target.value)} />
                        </div>
                        <div className='flex items-center gap-2'>
                            <FaGithub size={20} />
                            <Input placeholder='Github' value={formData.gitHubLink} onChange={(e) => handleChange('gitHubLink', e.target.value)} />
                        </div>
                        <div className='flex items-center gap-2'>
                            <FaLinkedin size={20} />
                            <Input placeholder='LinkedIn' value={formData.linkedInLink} onChange={(e) => handleChange('linkedInLink', e.target.value)} />
                        </div>
                    </div>
                </div>

                <div className='flex flex-col gap-4 w-full max-w-2xl'>
                    <h1 className='text-3xl font-bold'>Мій профіль</h1>

                    <div className='grid grid-cols-2 gap-4'>
                        <div className='flex flex-col'>
                            <label className='font-semibold'>Псевдонім</label>
                            <Input placeholder='Псевдонім' value={formData.realName} onChange={(e) => handleChange('realName', e.target.value)} />
                        </div>

                        <div className='flex flex-col col-span-1'>
                            <label className='font-semibold'>Група</label>
                            <Select value={formData.group} onValueChange={(value) => handleChange('group', value)}>
                                <SelectTrigger className='w-full'>
                                    <SelectValue placeholder='Група' />
                                </SelectTrigger>
                                <SelectContent>
                                    <SelectItem value='group-a'>Group A</SelectItem>
                                    <SelectItem value='group-b'>Group B</SelectItem>
                                    <SelectItem value='group-c'>Group C</SelectItem>
                                </SelectContent>
                            </Select>
                        </div>

                        <div className='flex flex-col'>
                            <label className='font-semibold'>Ім’я користувача</label>
                            <Input placeholder='Ім’я та прізвище' value={formData.userName} onChange={(e) => handleChange('userName', e.target.value)} />
                        </div>
                    </div>

                    <div className='flex flex-col'>
                        <label className='font-semibold'>Про себе</label>
                        <Textarea
                            placeholder='Напишіть щось про себе :)'
                            className='min-h-[120px]'
                            value={formData.aboutMyself}
                            onChange={(e) => handleChange('aboutMyself', e.target.value)}
                        />
                    </div>

                    <div className='flex justify-end gap-4 mt-4'>
                        <Button variant='outline' className='bg-gray-200 cursor-pointer'>Скасувати</Button>
                        <Button className='cursor-pointer' onClick={handleSave}>Зберегти</Button>
                    </div>
                </div>
            </div>

            <div className="absolute bottom-0 left-0 w-full h-full z-0">
                <Canvas>
                    <directionalLight position={[-10, -10, -10]} intensity={1} />
                    <ambientLight />
                    <Planet
                        position={[-5, -3, 2]}
                        size={2}
                        textureUrl="/planet_texture4.jpg"
                        clickable={false}
                    />
                    <OrbitControls />
                </Canvas>
            </div>
        </div>
    );
};

export default UpdateUserProfilePage;