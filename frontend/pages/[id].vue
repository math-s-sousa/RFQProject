<template>
    <div>
        <Loading v-if="loading" />
        <div class="container mt-5 rounded">
            <div class="row">
            <nav class="navbar bg-body-tertiary">
                <div class="container-fluid justify-content-start">
                    <img src="../assets/img/logo.png" alt="Logo" width="180" height="45" class="d-inline-block align-text-top">
                    <span class="m-3" id="title">Cotação Online</span>
                </div>
            </nav>
            </div>
            <form @submit.prevent="onSubmit">
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
                
                <div class="row mt-1 table-responsive" id="lines">
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
                            <td><input class="form-control" type="number" min="1" step="any" v-model="line.quantity"></td>
                            <td>
                            <div class="input-group">
                                <span class="input-group-text">{{ Document.docCurrency }}</span>
                                <input class="form-control" type="number" min="1" step="any" v-model="line.unitPrice">
                            </div>
                            </td>
                            <td><input class="form-control" type="text" v-model="line.freeText"></td>
                        </tr>
                        </tbody>
                    </table>
                </div>

                <div id="foot">
                    <button type="button" class="btn btn-danger m-2">Cancelar</button>
                    <button type="submit" class="btn btn-success m-2">Enviar</button>
                </div>
            </form>
        </div>
    </div>
</template>

<script setup>
    import { toast } from 'vue3-toastify';
    import 'vue3-toastify/dist/index.css';

    const { id } = useRoute().params
    const loading = ref(false)

    const { data: Document } = await useFetch(`/api/${id}`)
    Document.value.docDate = Document.value.docDate.substring(0,10)

    const updateQuotation = async () => {
        const { status } = await useAsyncData(() => { return $fetch(`/api/${id}`, { method: "POST", body: Document.value }) } )

        if (status.value === "success") {
            toast.success("Cotação atualizada!", {
                autoClose: 500,
                position: 'top-center',
                theme: 'colored',
                hideProgressBar: true
            })
        }
        else {
            toast.error("Erro ao devolver Cotação para o SAP, favor contatar empresa diretamente!", {
                autoClose: 3000,
                theme: 'colored',
                position: 'top-center',
                hideProgressBar: true
            })
        }
    }

    const onSubmit = async () => {
        loading.value = true
        await updateQuotation()
        loading.value = false
    }

</script>

<style scoped>
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

    #foot {
        text-align: end;
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