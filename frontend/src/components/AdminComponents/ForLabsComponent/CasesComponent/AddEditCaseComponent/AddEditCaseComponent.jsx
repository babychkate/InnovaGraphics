import React, { useEffect } from 'react';
import { useForm } from 'react-hook-form';
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
import { useDispatch, useSelector } from 'react-redux';
import { createCase, getAllCases, updateCase } from '@/redux/case/Action';
import { toast } from 'react-toastify';
import { getAllExercises } from '@/redux/exercise/Action';

const AddEditCaseComponent = ({ testCase, onBack }) => {
  const dispatch = useDispatch();
  const exercises = useSelector((state) => state.exercise.exercises);

  const form = useForm({
    defaultValues: {
      id: testCase?.id || 0,
      input: testCase?.input || '',
      output: testCase?.output || '',
      exerciseId: testCase?.exerciseId ? String(testCase.exerciseId) : '',
    },
  });

  const { handleSubmit } = form;

  useEffect(() => {
    dispatch(getAllExercises());
  }, [dispatch]);

  const onSubmit = async (data) => {
    if (!data.id) {
      const fromData = {
        id: data.id,
        input: data.input,
        output: data.output,
        exerciseId: data.exerciseId,
      }
      const { success } = await dispatch(createCase(fromData));
      if (success) {
        dispatch(getAllCases());
        onBack();
      }
    } else {
      const patches = [];

      const keyToPath = {
        input: '/input',
        output: '/output',
        exerciseId: '/exerciseId',
      };

      for (const key in data) {
        if (data[key] !== testCase[key] && keyToPath[key]) {
          patches.push({
            op: 'replace',
            path: keyToPath[key],
            value: data[key],
          });
        }
      }

      if (patches.length > 0) {
        const { success } = await dispatch(updateCase(testCase.id, patches));
        if (success) {
          dispatch(getAllCases());
          onBack();
        }
      } else {
        toast.info('Немає змін для збереження.');
      }
    }
  };

  return (
    <div className="relative min-h-[calc(100vh-200px)] flex items-center justify-center px-4">
      <Button
        variant="ghost"
        onClick={onBack}
        className="absolute -top-2 -left-2 z-20 p-2 rounded-full cursor-pointer transition hover:bg-gray-50"
      >
        <ArrowLeft size={28} />
      </Button>

      <div className="flex flex-col items-center justify-center gap-4">
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
                name="exerciseId"
                render={({ field }) => (
                  <FormItem className="flex flex-col">
                    <div className="flex items-center gap-4">
                      <FormLabel>Задача:</FormLabel>
                      <FormControl>
                        <Select
                          onValueChange={field.onChange}
                          defaultValue={field.value}
                        >
                          <SelectTrigger className="w-full">
                            <SelectValue placeholder="Оберіть задачу" />
                          </SelectTrigger>
                          <SelectContent>
                            {exercises.map((exercise) => (
                              <SelectItem key={exercise.id} value={String(exercise.id)}>
                                {exercise.name}
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
                name="input"
                render={({ field }) => (
                  <FormItem className="flex flex-col">
                    <div className="flex items-center gap-4">
                      <FormLabel className="whitespace-nowrap">Вхідні дані:</FormLabel>
                      <FormControl>
                        <Input placeholder="Вхідні дані ..." {...field} />
                      </FormControl>
                    </div>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="output"
                render={({ field }) => (
                  <FormItem className="flex flex-col">
                    <div className="flex items-center gap-4">
                      <FormLabel className="whitespace-nowrap">Очікувані дані:</FormLabel>
                      <FormControl>
                        <Input placeholder="Очікувані дані ..." {...field} />
                      </FormControl>
                    </div>
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
            {testCase?.id ? 'Зберегти' : 'Додати'}
          </Button>
          <Button className="cursor-pointer" variant="outline" onClick={onBack}>
            Скасувати
          </Button>
        </div>
      </div>
    </div>
  );
};

export default AddEditCaseComponent;