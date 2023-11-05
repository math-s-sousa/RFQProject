export default defineEventHandler(async (event) => {

    const guid = getRouterParam(event, 'guid')

    const uri = `http://localhost:6060/rfq/${guid}`

    const data = await $fetch(uri, {
        headers: {
            Authorization: 'Basic Q1VTVE9NUkZROjEyMzQ='
        }
    })
        
    return data
})