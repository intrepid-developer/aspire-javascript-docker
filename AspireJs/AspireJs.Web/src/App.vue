<template>
  <div class="min-h-screen bg-gray-100">
    <div class="container mx-auto py-8">
      <h1 class="text-4xl font-bold text-blue-600 mb-8 text-center">Movies Database</h1>
      <MovieList :movies="movies" @add-movie="openAddForm" @edit-movie="openEditForm" />
      <MovieForm v-if="showForm" :show="showForm" :movie="selectedMovie" @save="saveMovie" @close="closeForm" />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import MovieList from './MovieList.vue';
import MovieForm from './MovieForm.vue';

interface Movie {
  id: number;
  title: string;
  year: number;
  genre: string;
}

const movies = ref<Movie[]>([]);
const showForm = ref(false);
const selectedMovie = ref<Movie | undefined>(undefined);
const loading = ref(false);
const error = ref<string | null>(null);

async function fetchMovies() {
  loading.value = true;
  error.value = null;
  try {
    const res = await fetch('/api/movies');
    if (!res.ok) throw new Error('Failed to fetch movies');
    movies.value = await res.json() as Movie[];
  } catch (e: any) {
    error.value = e.message || 'Unknown error';
  } finally {
    loading.value = false;
  }
}

onMounted(fetchMovies);

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

async function saveMovie(movie: Omit<Movie, 'id'>) {
  if (selectedMovie.value) {
    // Edit existing
    const res = await fetch(`/api/movies/${selectedMovie.value.id}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(movie)
    });
    if (res.ok) {
      await fetchMovies();
    } else {
      alert('Failed to update movie');
    }
  } else {
    // Add new
    const res = await fetch('/api/movies', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(movie)
    });
    if (res.ok) {
      await fetchMovies();
    } else {
      alert('Failed to add movie');
    }
  }
  closeForm();
}
</script>

<style>
/* Add component styles here */
</style>

