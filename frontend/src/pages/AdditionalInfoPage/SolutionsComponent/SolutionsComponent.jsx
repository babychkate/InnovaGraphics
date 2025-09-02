import { Input } from '@/components/ui/input';
import { Heart, Search } from 'lucide-react';
import {
    Select,
    SelectContent,
    SelectGroup,
    SelectItem,
    SelectTrigger,
    SelectValue,
} from "@/components/ui/select";
import React from 'react';

const solutions = [
    {
        id: 1,
        name: 'Килим Серпінського - який тип фракталу?',
        theme: "Фрактальна графіка",
        description: "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud лол ullamco laboris nisi ut aliquip ex ea commodo consequat.",
        isLicked: true,
        image: "/solutions/solutions-1.png",
    },
    {
        id: 2,
        name: 'Khan Academy',
        theme: "Засоби комп’ютерної графіки",
        description: "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud лол ullamco laboris nisi ut aliquip ex ea commodo consequat.",
        isLicked: true,
        image: "/solutions/solutions-2.png",
    },
    {
        id: 3,
        name: 'Color Matters',
        theme: "Програмування рухомих зображень",
        description: "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud лол ullamco laboris nisi ut aliquip ex ea commodo consequat.",
        isLicked: false,
        image: "/solutions/solutions-3.png",
    },
];

const SolutionsComponent = () => {
    return (
        <div className='min-h-[calc(100vh-121px)] bg-[#85A7FA] flex flex-col gap-4 py-8'>
            <h1 className='text-2xl font-semibold text-center'>
                Надихайтеся та Вчіться: Готові реалізації цікавих завдань
            </h1>
            <div className='flex items-center px-4 gap-4'>
                <div className='relative w-full max-w-sm'>
                    <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-500 w-5 h-5" />
                    <Input className="pl-10 bg-white placeholder-gray-500" placeholder="Шукати за назвою" />
                </div>
                <div>
                    <Select>
                        <SelectTrigger className="cursor-pointer bg-white w-[250px]">
                            <SelectValue placeholder="Фільтрувати за темою" />
                        </SelectTrigger>
                        <SelectContent>
                            <SelectGroup>
                                <SelectItem value="theme-1">Тема 1</SelectItem>
                                <SelectItem value="theme-2">Тема 2</SelectItem>
                                <SelectItem value="theme-3">Тема 3</SelectItem>
                                <SelectItem value="theme-4">Тема 4</SelectItem>
                                <SelectItem value="theme-5">Тема 5</SelectItem>
                            </SelectGroup>
                        </SelectContent>
                    </Select>
                </div>
            </div>

            <div className='flex flex-col gap-4 px-4'>
                {/* Here will be redirect to other site !!!!!!! */}
                {solutions.map((solution) => (
                    <div key={solution.id} className="cursor-pointer group">
                        <div className="relative flex items-start bg-[#C2D3FD] p-4 rounded-4xl shadow-md overflow-hidden transition hover:scale-[1.01]">
                            <Heart
                                className={`absolute top-4 right-4 w-5 h-5 ${solution.isLicked ? "text-red-500" : "text-gray-400"}`}
                            />
                            <img
                                src={solution.image}
                                alt={solution.name}
                                className="w-10 h-10 object-cover rounded-4xl"
                            />
                            <div className="px-4 flex flex-col">
                                <h2 className="text-lg font-semibold">{solution.name}</h2>
                                <p className="text-sm text-gray-600">{solution.theme}</p>
                                <p className="text-sm mt-2 text-gray-700 line-clamp-3 max-w-[1000px]">
                                    {solution.description}
                                </p>
                            </div>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
}

export default SolutionsComponent;