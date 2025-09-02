import { Input } from '@/components/ui/input';
import { Search } from 'lucide-react';
import React, { useEffect } from 'react';
import FilterDropdown from './FilterDropdown/FilterDropdown';
import { useDispatch, useSelector } from 'react-redux';
import { getUserPurchases } from '@/redux/shop_items/Action';

const YourPurchases = () => {
    const dispatch = useDispatch();
    const user = useSelector(state => state.auth.user);
    const purchases = useSelector(state => state.shopItems.shopItems);

    useEffect(() => {
        dispatch(getUserPurchases(user?.id))
    }, [dispatch]);

    return (
        <div className='min-h-[calc(100vh-141px)] bg-[#85A7FA] flex flex-col gap-4 py-8'>
            <h1 className='text-2xl font-semibold text-center'>ВАШІ ПОКУПКИ</h1>
            <div>
                <div className='flex items-center justify-between px-4'>
                    <div className='relative w-full max-w-sm'>
                        <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-500 w-5 h-5" />
                        <Input
                            className="pl-10 bg-white placeholder-gray-500"
                            placeholder="Пошук за назвою"
                        />
                    </div>
                    <div className='flex items-center justify-between gap-4'>
                        <FilterDropdown
                            label="Фільтрувати за типом"
                            options={[
                                { label: 'Фони програми', value: 'app-theme' },
                                { label: 'Музичні теми', value: 'music-theme' },
                                { label: 'Планети', value: 'planet' },
                                { label: 'Аватари', value: 'avatar' },
                                { label: 'Ресурси', value: 'resource' },
                                { label: 'Підказки', value: 'hint' },
                                { label: 'Доступ до планет', value: 'access-to-planet' },
                                { label: 'Приклади рішень', value: 'examples' },
                            ]}
                        />
                        <FilterDropdown
                            label="Сортувати за"
                            options={[
                                { label: 'Назва', value: 'name' },
                                { label: 'Ціна', value: 'price' },
                                { label: 'Дата покупки', value: 'bougth-date' },
                                { label: 'Від меншого до більшого', value: 'less-to-greater' },
                            ]}
                        />
                    </div>
                </div>
                <div className="mt-6 px-4 grid gap-y-4">
                    <div className="grid grid-cols-3 font-semibold text-xl">
                        <div>Продукт</div>
                        <div>Ціна</div>
                        <div>Дата купівлі</div>
                    </div>
                    {purchases?.map((purchase) => (
                        <div key={purchase?.id} className="grid grid-cols-3 items-center bg-[#C2D3FD] rounded-4xl px-6 py-4 gap-4">
                            <div className="flex items-center gap-4">
                                <img src={purchase?.photoPath} alt={purchase?.shopItemName} className="w-12 h-12 object-cover rounded-full" />
                                <div>
                                    <div className="font-medium">{purchase?.shopItemName}</div>
                                    <div className="text-sm text-gray-600">{purchase?.subname}</div>
                                </div>
                            </div>
                            <div className="flex items-center gap-2 text-md font-bold">
                                {purchase?.price}
                                <img src="/coin.png" alt="coin" className="w-6 h-6" />
                            </div>
                            <div>{purchase?.purchaseDate}</div>
                        </div>
                    ))}
                </div>
            </div>
        </div>
    );
}

export default YourPurchases;