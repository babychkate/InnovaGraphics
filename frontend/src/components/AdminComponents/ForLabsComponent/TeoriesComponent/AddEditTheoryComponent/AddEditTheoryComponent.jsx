import { getAllPlanets } from '@/redux/planet/Action';
import React, { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { useDispatch, useSelector } from 'react-redux';
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { ArrowLeft } from 'lucide-react';
import MyEditor from '@/components/MyEditor/MyEditor';
import { createTheory, getAllTheories, updateTheory } from '@/redux/theory/Action';
import { toast } from 'react-toastify';

const AddEditTheoryComponent = ({ theory, onBack }) => {
  console.log(theory);
  const dispatch = useDispatch();
  const planets = useSelector((state) => state?.planet?.planets || []);

  const form = useForm({
    defaultValues: {
      id: theory?.id || 0,
      content: theory?.content || '',
      planetId: theory?.planetId ? String(theory.planetId) : '',
    },
  });

  const { handleSubmit } = form;

  const onSubmit = async (data) => {
    console.log('Submitted theory:', data);
    if (!data.id) {
      const { success } = await dispatch(createTheory({ content: data.content, planetId: data.planetId }));
      if (success) {
        dispatch(getAllTheories());
        onBack();
      }
    } else {
      const patches = [];

      const keyToPath = {
        content: '/content',
        planetId: '/planetId',
      };

      for (const key in data) {
        if (data[key] !== theory[key] && keyToPath[key]) {
          patches.push({
            op: 'replace',
            path: keyToPath[key],
            value: data[key],
          });
        }
      }

      if (patches.length > 0) {
        const { success } = await dispatch(updateTheory(theory.id, patches));
        if (success) {
          dispatch(getAllTheories());
          onBack();
        }
      } else {
        toast.info('Немає змін для збереження.');
      }
    }
  };

  useEffect(() => {
    dispatch(getAllPlanets());
  }, [dispatch]);

  return (
    <div className="relative min-h-[calc(100vh-200px)] flex items-center justify-center px-4">
      <Button
        variant="ghost"
        onClick={onBack}
        className="absolute -top-2 -left-2 z-20 p-2 rounded-full cursor-pointer transition hover:bg-gray-50"
      >
        <ArrowLeft size={28} />
      </Button>

      <div className="flex flex-col items-center justify-center gap-4 w-full max-w-3xl">
        <div className="flex items-center justify-center w-full">
          <Form {...form}>
            <form
              onSubmit={handleSubmit(onSubmit)}
              className="flex flex-col gap-4 w-full"
              id="theory-form"
            >
              <FormField
                control={form.control}
                name="id"
                render={({ field }) => (
                  <FormItem className="hidden">
                    <FormControl>
                      <Input type="hidden" {...field} />
                    </FormControl>
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="planetId"
                render={({ field }) => (
                  <FormItem className="flex flex-col">
                    <div className="flex items-center gap-4">
                      <FormLabel>Планета:</FormLabel>
                      <FormControl className="flex-1">
                        <Select
                          onValueChange={field.onChange}
                          defaultValue={field.value}
                        >
                          <SelectTrigger>
                            <SelectValue placeholder="Оберіть планету" />
                          </SelectTrigger>
                          <SelectContent>
                            {planets.map((planet) => (
                              <SelectItem key={planet.id} value={String(planet.id)}>
                                {planet.name}
                              </SelectItem>
                            ))}
                          </SelectContent>
                        </Select>
                      </FormControl>
                    </div>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="content"
                render={({ field }) => (
                  <FormItem className="flex flex-col gap-2 w-full">
                    <FormLabel>Контент:</FormLabel>
                    <FormControl>
                      <MyEditor value={field.value} onChange={field.onChange} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
            </form>
          </Form>
        </div>

        <div className="flex items-end justify-end gap-4 w-full mt-4">
          <Button
            className="text-sm px-5 py-2 bg-[#2354E1] hover:bg-[#2369e1] text-white"
            type="submit"
            form="theory-form"
          >
            {theory?.id ? 'Зберегти' : 'Додати'}
          </Button>
          <Button className="cursor-pointer" variant="outline" onClick={onBack}>
            Скасувати
          </Button>
        </div>
      </div>
    </div>
  );
};

export default AddEditTheoryComponent;