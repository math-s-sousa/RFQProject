<script setup>
import { ref } from 'vue';
import { useRouter, useRoute } from 'vue-router'
import { getQuotation, postQuotation } from '../services/quotations'
import { toast } from 'vue3-toastify';
import 'vue3-toastify/dist/index.css';

const router = useRouter();
const route = useRoute();

const Document = ref(null);

getData()

async function test() {
  console.log("test")
}

async function getData() {
  try {
    const get = ref(await getQuotation(route.params.id))
    Document.value = get.value.data
    Document.value.docDate = Document.value.docDate.substring(0,10)
  }
  catch(err) {
    router.push("/")
  }
}

async function postData(document) {
  try {
    const post = ref(await postQuotation(route.params.id, document))

    if (post.value.data.status === 200) {
      toast.success("Cotação atualizada!", {
        autoClose: 500,
        position: 'top-center',
        theme: 'colored',
        hideProgressBar: true
      })
    }

    else {
      toast.error("Problemas ao enviar, favor contatar diretamente.", {
        autoClose: 3000,
        theme: 'colored',
        position: 'top-center',
        hideProgressBar: true
      })
    }
  }
  catch(err) {
    toast.error(err.response.data.detail, {
      autoClose: 3000,
      theme: 'colored',
      position: 'top-center',
      hideProgressBar: true
      })
  }
}
</script>

<template>
  <div class="container mt-5 rounded" v-if="Document">
    <div class="row">
      <nav class="navbar bg-body-tertiary">
        <div class="container-fluid">
          <a class="navbar-brand">
            <img src="../assets/img/sap-business-one-logo-banner.png" alt="Logo" width="180" height="45" class="d-inline-block align-text-top">
            <span class="m-3" id="title">Cotação Online</span>
          </a>
        </div>
      </nav>
    </div>

    <div class="row mt-1" id="head">
      <div class="col-6 mt-3">
        <label class="form-label">Fornecedor</label>
        <input type="text" class="form-control" alt="Fornecedor" v-model="Document.cardName" disabled>
      </div>  
      <div class="col-6 mt-3 mb-3">
        <label class="form-label">Data do Documento</label>
        <input type="date" class="form-control" alt="Data Documento" v-model="Document.docDate" disabled>
      </div>  
    </div>
    
    <div class="row mt-1" id="lines">
      <table class="table mt-1">
        <thead>
          <tr>
            <th scope="col">#</th>
            <th scope="col">Item</th>
            <th scope="col">Quantidade</th>
            <th scope="col">Preço Unitário</th>
            <th scope="col">Observações</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="line in Document.documentLines">
            <th scope="row">{{ line.visualOrder + 1 }}</th>
            <td>{{ line.itemDescription }}</td>
            <td><input class="form-control" type="number" v-model="line.quantity"></td>
            <td>
              <div class="input-group">
                <span class="input-group-text">{{ line.currency }}</span>
                <input class="form-control" type="number" v-model="line.unitPrice">
              </div>
              </td>
            <td><input class="form-control" type="text" v-model="line.freeText"></td>
          </tr>
        </tbody>
      </table>
    </div>

    <div class="row d-flex justify-content-end" id="foot">
      <button class="btn btn-danger col-1 m-2" @click="test">Cancelar</button>
      <button class="btn btn-success col-1 m-2" @click="postData(Document)">Enviar</button>
    </div>
  </div>
  <div v-else class="loading">
    Carregando...
  </div>
</template>

<style>
#title {
  color: #2e9abf;
  font-weight: bold;
  font-size: xx-large;
  vertical-align: text-top;
}

#head, #lines {
  background-color: white;
  border-radius: 6px;
}

.form-label {
  font-weight: bold;
}

.loading {
  text-align: center;
  font-weight: bold;
  font-size: xx-large;
}

.btn-success {
  background-color: #2e9abf;
  border-color: #2e9abf;
}

.btn-success:hover {
  background-color: #2e9abf;
  border-color: #2e9abf;
  border-radius: 8px;
  box-shadow: 0 0 5px rgba(0, 0, 0, 0.3);
}

.btn-danger {
  background-color: rgb(168, 160, 159);
  border-color: rgb(168, 160, 159);
}

.btn-danger:hover {
  background-color: rgb(168, 160, 159);
  border-color: rgb(168, 160, 159);
  border-radius: 8px;
  box-shadow: 0 0 5px rgba(0, 0, 0, 0.3);
}
</style>