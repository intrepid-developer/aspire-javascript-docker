<template>
  <div class="fixed inset-0 bg-black bg-opacity-30 flex items-center justify-center z-50">
    <div class="bg-white rounded-lg shadow-lg p-8 w-full max-w-md relative">
      <button class="absolute top-2 right-2 text-gray-400 hover:text-gray-600" @click="$emit('close')">&times;</button>
      <h2 class="text-xl font-bold mb-4">{{ isEdit ? 'Edit Movie' : 'Add Movie' }}</h2>
      <form @submit.prevent="onSubmit">
        <div class="mb-4">
          <label class="block mb-1 font-medium">Title</label>
          <input v-model="form.title" class="w-full border rounded px-3 py-2" required />
        </div>
        <div class="mb-4">
          <label class="block mb-1 font-medium">Year</label>
          <input v-model.number="form.year" type="number" class="w-full border rounded px-3 py-2" required min="1888" />
        </div>
        <div class="mb-4">
          <label class="block mb-1 font-medium">Genre</label>
          <input v-model="form.genre" class="w-full border rounded px-3 py-2" required />
        </div>
        <div class="flex justify-end">
          <button type="button" class="mr-2 px-4 py-2 rounded border" @click="$emit('close')">Cancel</button>
          <button type="submit" class="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700">{{ isEdit ? 'Save' : 'Add' }}</button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { reactive, watch, toRefs } from 'vue';

const props = defineProps<{
  show: boolean;
  movie?: { id: number; title: string; year: number; genre: string };
}>();
const emit = defineEmits(['save', 'close']);

const isEdit = !!props.movie;
const form = reactive({
  title: props.movie?.title || '',
  year: props.movie?.year || new Date().getFullYear(),
  genre: props.movie?.genre || '',
});

watch(() => props.movie, (newMovie) => {
  if (newMovie) {
    form.title = newMovie.title;
    form.year = newMovie.year;
    form.genre = newMovie.genre;
  } else {
    form.title = '';
    form.year = new Date().getFullYear();
    form.genre = '';
  }
});

function onSubmit() {
  emit('save', { ...form });
}
</script>

