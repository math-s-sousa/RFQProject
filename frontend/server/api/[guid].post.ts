export default defineEventHandler(async (event) => {

    const guid = getRouterParam(event, 'guid')
    const body = await readBody(event)

    const uri = `http://localhost:6060/rfq/${guid}`

    const data = await $fetch(uri, {
        method: 'POST',
        headers: {
            Authorization: 'Basic Q1VTVE9NUkZROjEyMzQ='
        },
        body: body
    })
        
    return data
})