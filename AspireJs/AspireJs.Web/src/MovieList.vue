<template>
  <div class="container mx-auto py-8">
    <div class="flex justify-between items-center mb-6">
      <h2 class="text-2xl font-bold">Movie List</h2>
      <button class="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700" @click="$emit('add-movie')">Add Movie</button>
    </div>
    <table class="min-w-full bg-white rounded shadow">
      <thead>
        <tr>
          <th class="py-2 px-4 border-b">Title</th>
          <th class="py-2 px-4 border-b">Year</th>
          <th class="py-2 px-4 border-b">Genre</th>
          <th class="py-2 px-4 border-b">Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="movie in movies" :key="movie.id">
          <td class="py-2 px-4 border-b">{{ movie.title }}</td>
          <td class="py-2 px-4 border-b">{{ movie.year }}</td>
          <td class="py-2 px-4 border-b">{{ movie.genre }}</td>
          <td class="py-2 px-4 border-b">
            <button class="text-blue-600 hover:underline mr-2" @click="$emit('edit-movie', movie)">Edit</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup lang="ts">
defineProps<{ movies: { id: number; title: string; year: number; genre: string }[] }>()
</script>

