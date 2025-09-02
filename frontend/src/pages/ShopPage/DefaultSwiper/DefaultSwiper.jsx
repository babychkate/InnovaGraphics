import React from 'react';

import { Swiper, SwiperSlide } from 'swiper/react';
import { EffectCoverflow } from 'swiper/modules';
import 'swiper/css';
import 'swiper/css/effect-coverflow';
import 'swiper/css/pagination';

const DefaultSwiper = () => {
    const images = [
        "interstellar.png",
        "space.png",
        "nebula.png",
    ];

    return (
        <div className="flex items-center justify-center flex-col gap-4 py-4">
            <h1 className="text-3xl text-center">
                <span className="font-bold">НОВИНКИ</span> В INNOVASTORE
            </h1>
            <div className="w-full pt-12 pb-12">
                <Swiper
                    effect={'coverflow'}
                    grabCursor={true}
                    centeredSlides={true}
                    slidesPerView={'auto'}
                    initialSlide={Math.round(images.length / 2 - 1)}
                    coverflowEffect={{
                        rotate: 0,
                        stretch: 0,
                        depth: 200,
                        modifier: 2,
                        slideShadows: false,
                    }}
                    modules={[EffectCoverflow]}
                    className="w-full"
                >
                    {images.map((img, i) => (
                        <SwiperSlide
                            key={i}
                            className="!w-[375px] !h-[375px] bg-center bg-cover flex items-center justify-center"
                        >
                            <img
                                src={`/${img}`}
                                alt={`Slide ${i + 1}`}
                                className="block w-full h-full object-cover rounded-xl shadow-xl"
                            />
                        </SwiperSlide>
                    ))}
                </Swiper>
                <p className="text-center text-lg mt-4">
                    Interstellar - Hans Zimmer - Main Theme
                </p>
            </div>
        </div>
    );
}

export default DefaultSwiper;