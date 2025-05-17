<template>
  <div class="min-h-screen bg-gray-100">
    <div class="container mx-auto py-8">
      <h1 class="text-4xl font-bold text-blue-600 mb-8 text-center">Vue 3 + Vite + Tailwind CSS Movie Database</h1>
      <MovieList :movies="movies" @add-movie="openAddForm" @edit-movie="openEditForm" />
      <MovieForm v-if="showForm" :show="showForm" :movie="selectedMovie" @save="saveMovie" @close="closeForm" />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import MovieList from './MovieList.vue';
import MovieForm from './MovieForm.vue';

interface Movie {
  id: number;
  title: string;
  year: number;
  genre: string;
}

const movies = ref<Movie[]>([
  { id: 1, title: 'The Shawshank Redemption', year: 1994, genre: 'Drama' },
  { id: 2, title: 'The Godfather', year: 1972, genre: 'Crime' },
  { id: 3, title: 'Inception', year: 2010, genre: 'Sci-Fi' },
]);

const showForm = ref(false);
const selectedMovie = ref<Movie | undefined>(undefined);

function openAddForm() {
  selectedMovie.value = undefined;
  showForm.value = true;
}

function openEditForm(movie: Movie) {
  selectedMovie.value = { ...movie };
  showForm.value = true;
}

function closeForm() {
  showForm.value = false;
  selectedMovie.value = undefined;
}

function saveMovie(movie: Omit<Movie, 'id'>) {
  if (selectedMovie.value) {
    // Edit existing
    const idx = movies.value.findIndex(m => m.id === selectedMovie.value!.id);
    if (idx !== -1) {
      movies.value[idx] = { ...selectedMovie.value, ...movie };
    }
  } else {
    // Add new
    const newId = Math.max(...movies.value.map(m => m.id), 0) + 1;
    movies.value.push({ id: newId, ...movie });
  }
  closeForm();
}
</script>

<style>
/* Add component styles here */
</style>

