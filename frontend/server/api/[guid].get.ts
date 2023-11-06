export default defineEventHandler(async (event) => {

    const config = useRuntimeConfig()

    const guid = getRouterParam(event, 'guid')

    const uri = `${config.apiBaseUrl}/rfq/${guid}`

    const data = await $fetch(uri, {
        headers: {
            Authorization: `Basic ${btoa(config.apiUser + ':' + config.apiPass)}`
        }
    })
        
    return data
})