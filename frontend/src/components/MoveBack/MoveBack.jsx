import { ArrowLeft } from 'lucide-react';
import React from 'react';
import { Link } from 'react-router-dom';

const MoveBack = ({ to }) => {
    return (
        <Link
            to={to}
            className='absolute top-23 left-5 z-20 p-2 hover:bg-gray-200 rounded-full transition-all duration-300 ease-in-out'
        >
            <ArrowLeft size={28} />
        </Link>
    );
}

export default MoveBack;