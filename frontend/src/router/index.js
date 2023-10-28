import { createRouter, createWebHistory } from 'vue-router'
import RFQView from '../views/RFQView.vue'
import NotFound from '../views/NotFound.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/:id',
      name: 'home',
      component: RFQView
    },
    {
      path: "/:catchAll(.*)",
      name: "NotFound",
      component: NotFound,
    }
  ]
})

export default router
