import { Button } from '@/components/ui/button';
import { Checkbox } from '@/components/ui/checkbox';
import { Label } from '@/components/ui/label';
import { Popover, PopoverContent, PopoverTrigger } from '@/components/ui/popover';
import { ChevronDown, ChevronUp } from 'lucide-react';
import React, { useState } from 'react';

const FilterDropdown = ({ label = "Фільтр", options = [] }) => {
    const [selected, setSelected] = useState([]);
    const [isOpen, setIsOpen] = useState(false);

    const toggleOption = (value) => {
        setSelected((prev) =>
            prev.includes(value) ? prev.filter((v) => v !== value) : [...prev, value]
        );
    };

    return (
        <Popover open={isOpen} onOpenChange={setIsOpen}>
            <PopoverTrigger asChild>
                <Button
                    variant="outline"
                    className="bg-white text-black w-[250px] justify-between"
                >
                    <span>{label}</span>
                    {isOpen ? <ChevronUp className="w-4 h-4 ml-2" /> : <ChevronDown className="w-4 h-4 ml-2" />}
                </Button>
            </PopoverTrigger>
            <PopoverContent className="w-[250px]">
                <div className="flex flex-col gap-2">
                    {options.map((option) => (
                        <div key={option.value} className="flex items-center gap-2">
                            <Checkbox
                                id={option.value}
                                checked={selected.includes(option.value)}
                                onCheckedChange={() => toggleOption(option.value)}
                            />
                            <Label htmlFor={option.value}>{option.label}</Label>
                        </div>
                    ))}
                </div>
            </PopoverContent>
        </Popover>
    );
};

export default FilterDropdown;