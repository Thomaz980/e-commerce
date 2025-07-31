<script setup lang="ts">
  import { IonPage, IonHeader, IonToolbar, IonTitle, IonContent, IonButtons, IonIcon } from '@ionic/vue';
  import ExploreContainer from '@/components/ExploreContainer.vue';
  import { bagOutline } from 'ionicons/icons';

  function onMenuClick() {
    
  }
</script>

<template>
  <ion-page>
    <ion-header>
        <ion-buttons slot="start">
          <button class="menu-hamburguer" @click="onMenuClick">
            <span class="hamburguer-icon">&#9776;</span>
          </button>
        </ion-buttons>
        <ion-title slot="title" class="text-white text-center">EXIBBA</ion-title>
        <ion-buttons slot="end" class="flex items-center justify-center">
          <ion-icon class="text-white" :icon="bagOutline" size="large"></ion-icon>
        </ion-buttons>
    </ion-header>
    <ion-content :fullscreen="true">
      <ExploreContainer name="Tab 1 page" />
    </ion-content>
  </ion-page>
</template>


<style scoped>

.title {
  font-size: 1.5rem;
  font-weight: bold;
  text-align: center;
  position: relative;
  top: -1rem; 
  z-index: 1;
}

.toolbar-center {
  position: absolute;
  left: 0;
  right: 0;
  top: 0;
  bottom: 0;
  display: flex;
  align-items: center;
  text-align: center;
  justify-content: center;
  pointer-events: none;
  height: 100%;
}

.toolbar-center ion-title {
  margin: 0;
  font-size: 1.2rem;
  font-weight: 500;
  text-align: center;
}

.menu-hamburguer {
  background: none;
  border: none;
  cursor: pointer;
  padding: 8px;
  display: flex;
  align-items: center;
}

.hamburguer-icon {
  font-size: 2rem;
  font-weight: bold;
}

</style>
