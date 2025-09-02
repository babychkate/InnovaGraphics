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

const usefulSites = [
    {
        id: 1,
        name: 'Paul Bourke - Personal Pages',
        description: "Lorem ipsum dolor sit ametLorem ipsum dolor sit amet...Lorem ipsum dolor sit amet...Lorem ipsum dolor sit amet...Lorem ipsum dolor sit amet...Lorem ipsum dolor sit amet...",
        themes: ["3D", "2D"],
        isLicked: true,
        image: "https://cdn.kastatic.org/images/favicon.ico?logo",
    },
    {
        id: 2,
        name: 'Khan Academy',
        description: "Lorem ipsum dolor sit amet...Lorem ipsum dolor sit amet...Lorem ipsum dolor sit amet...Lorem ipsum dolor sit amet...Lorem ipsum dolor sit amet...Lorem ipsum dolor sit amet...",
        themes: ["Комп'ютерна графіка", "2D"],
        isLicked: true,
        image: "/site-icon/site-icon-2.png",
    },
    {
        id: 3,
        name: 'Color Matters',
        description: "Lorem ipsum dolor sit amet...",
        themes: ["Фрактальна графіка", "2D", "3D"],
        isLicked: false,
        image: "/site-icon/site-icon-3.png",
    },
];

const UsefulSitesComponent = () => {
    return (
        <div className='min-h-[calc(100vh-121px)] bg-[#85A7FA] flex flex-col gap-4 py-8'>
            <h1 className='text-2xl font-semibold text-center'>
                Відкрийте для себе світ Комп’ютерної графіки: Корисні Сайти
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
                {usefulSites.map((site) => (
                    <div key={site.id} className="cursor-pointer group">
                        <div className="relative flex items-start bg-[#C2D3FD] p-4 rounded-4xl shadow-md overflow-hidden transition hover:scale-[1.01]">
                            <Heart
                                className={`absolute top-4 right-4 w-5 h-5 ${site.isLicked ? "text-red-500" : "text-gray-400"}`}
                            />
                            <img
                                src={site.image}
                                alt={site.name}
                                className="w-10 h-10 object-cover rounded-4xl"
                            />
                            <div className="px-4 flex flex-col">
                                <h2 className="text-lg font-bold">{site.name}</h2>

                                <div className="flex flex-wrap gap-2 mt-1">
                                    {site.themes.map((theme, index) => (
                                        <span
                                            key={index}
                                            className="text-sm font-semibold text-gray-700"
                                        >
                                            {theme}
                                        </span>
                                    ))}
                                </div>

                                <p className="text-sm mt-2 text-gray-700 line-clamp-3 max-w-[1000px]">
                                    {site.description}
                                </p>
                            </div>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
}

export default UsefulSitesComponent;